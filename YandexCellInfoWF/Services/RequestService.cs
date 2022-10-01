using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YandexCellInfoWF.Models;
using System.Windows.Forms;
using System.Threading;

namespace YandexCellInfoWF.Services
{
    public class RequestService
    {
        private static CancellationTokenSource ctSource;
        private static CancellationToken ct;

        private static HttpClient client = new HttpClient();

        public static async Task<BaseItemInfo> MakeRequest(TextBox console, YandexRequestCommonInfo commonInfo, List<CellInfo> cells, int sectorNum = -1, bool repeated = false)
        {
            ctSource = new CancellationTokenSource();
            ct = ctSource.Token; //удОлить как можно скорее



            var compressedData = GzipService.compressString(JsonConvert.SerializeObject(new YandexRequest(commonInfo, cells)));

            var content = new MultipartFormDataContent("YaBoundary");
            content.Add(new StringContent("\n\t"), "gzip");
            content.Add(new ByteArrayContent(compressedData), "json");


            var parsedResponse = new YandexResponse();
            var result = new BaseItemInfo();
            try
            {

                var response = await client.PostAsync("http://api.lbs.yandex.net/geolocation", content).Result.Content.ReadAsByteArrayAsync();
                var decompressedResponse = Encoding.UTF8.GetString(GzipService.decompressBytes(response));
                parsedResponse = JObject.Parse(decompressedResponse)["position"].ToObject<YandexResponse>();

                if (parsedResponse.LocationType.ToLower() == "gsm")
                {
                    result = new BaseItemInfo(sectorNum, parsedResponse);
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception e)
            {
                if (repeated)
                {
                    console.AppendText($"\r\n[{DateTime.Now:T}] Произошла ошибка {e.Message} Пропуск БС.");
                    return result;
                }
                console.AppendText($"\r\n[{DateTime.Now:T}] Произошла ошибка {e.Message} Повтор.");
                return await MakeRequest(console, commonInfo, cells, sectorNum, true);
            }
        }
    }
}
