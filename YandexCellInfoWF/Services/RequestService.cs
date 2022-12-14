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
using System.Diagnostics;

namespace YandexCellInfoWF.Services
{
    public class RequestService
    {
        private static HttpClient client = new HttpClient();

        public static async Task<BaseItemInfo> MakeRequest(TextBox console, YandexRequestCommonInfo commonInfo, List<CellInfo> cells, CancellationToken ct, int sectorNum = -1, bool repeated = false)
        {
            return await Task.Run(async () =>
            {
                var result = new BaseItemInfo();

                try
                {
                    dynamic content;
                    if (cells.Count < 1000)
                    {
                        var request = JsonConvert.SerializeObject(new YandexRequest(commonInfo, cells));
                        content = new StringContent("json=" + request, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        content = new MultipartFormDataContent("YaBoundary");

                        var compressedData = await GzipService.CompressString(JsonConvert.SerializeObject(new YandexRequest(commonInfo, cells)));
                        content.Add(new StringContent("gzip"), "\"gzip\"");
                        content.Add(new ByteArrayContent(compressedData), "\"json\"");

                    }
                    var parsedResponse = new YandexResponse();

                    ct.ThrowIfCancellationRequested();

                    var response = await client.PostAsync("http://api.lbs.yandex.net/geolocation", content).Result.Content.ReadAsStringAsync();

                    parsedResponse = JObject.Parse(response)["position"].ToObject<YandexResponse>();

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
                        //console.AppendText($"\r\n[{DateTime.Now:T}] Произошла ошибка {e.Message} Пропуск БС.");
                        return result;
                    }
                    //console.AppendText($"\r\n[{DateTime.Now:T}] Произошла ошибка {e.Message} Повтор.");
                    return await MakeRequest(console, commonInfo, cells, ct, sectorNum, true);
                }
            });
        }
    }
}
