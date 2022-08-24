using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YandexCellInfoWF.Models;

namespace YandexCellInfoWF.Workers
{
    public static class ManyInfoWorker
    {
        private static CancellationTokenSource ctSource;
        private static CancellationToken ct;

        private static HttpClient client = new HttpClient();

        public static async Task<bool> SearchEnbs(TextBox console, ProgressBar progressBar, Label currentEnb, string apiKey, string mccString, string mncString, string enbsString, string lacsString, string sectorsString)
        {
            ctSource = new CancellationTokenSource();
            ct = ctSource.Token;

            console.Text = $"[{DateTime.Now:T}] Начат поиск всех БС по заданным параметрам.";

            var parsedData = new OutputData();
            var commonInfo = new YandexRequestCommonInfo(apiKey);
            var results = new List<EnbShortInfo>();

            if (!InputParser.ParseInputWithSector(new InputData(mccString, mncString, enbsString, lacsString, sectorsString), out parsedData))
            {
                console.AppendText("\r\nОшибка в входных данных. Завершение работы алгоритма.");
                return false;
            }

            //var counter = 0;

            //foreach (var enb in parsedData.Enbs)
            for (var i = 0; i < parsedData.Enbs.Count; i++)
            {
                //counter++;
                var cells = new List<CellInfo>();
                foreach (var lac in parsedData.Lacs)
                {
                    foreach (var sector in parsedData.Sectors)
                        cells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enbNumber: parsedData.Enbs[i], sector: sector));
                }
                var request = JsonConvert.SerializeObject(new YandexRequest(commonInfo, cells));
                var content = new StringContent("json=" + request, Encoding.UTF8, "application/json");

                try
                {
                    var parsedResponse = new YandexResponse();

                    await Task.Run(() =>
                    {
                        ct.ThrowIfCancellationRequested();

                        var response = client.PostAsync("http://api.lbs.yandex.net/geolocation", content).Result.Content.ReadAsStringAsync().Result;
                        parsedResponse = JObject.Parse(response)["position"].ToObject<YandexResponse>();
                    }, ctSource.Token);
                    if (parsedResponse.LocationType.ToLower() == "gsm")
                    {
                        results.Add(new EnbShortInfo(parsedData.Enbs[i], parsedResponse));
                        console.AppendText($"\r\n[{DateTime.Now:T}] Найдено! Enb: {parsedData.Enbs[i]}." +
                            $"\r\nGPS: {parsedResponse.Latitude:0.00000}, {parsedResponse.Longitude:0.00000}");
                        console.ScrollToCaret();
                    }
                }
                catch (OperationCanceledException)
                {
                    System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{DateTime.Now.Hour}.{DateTime.Now.Minute} {mccString}-{mncString}" + "EnbAllInfo.txt", JsonConvert.SerializeObject(results, Formatting.Indented));
                    //System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{DateTime.Now.Hour}.{DateTime.Now.Minute} {mccString}-{mncString}" + "EnbNums.txt", JsonConvert.SerializeObject(successEnbsNums, Formatting.Indented));
                    console.AppendText($"\n\t[{DateTime.Now:T}] Поиск сот принудительно остановлен. Все найденные соты помещены в файлы EnbAllInfo.txt и EnbNums.txt.");

                    return true;
                }
                catch (AggregateException exeption)
                {
                    var exeptions = exeption.InnerExceptions;
                    if (exeptions.Any(e => e.GetType() == typeof(HttpRequestException)))
                        console.AppendText($"\r\n[{DateTime.Now:T}] Произошла ошибка получения данных. Пропуск БС #{parsedData.Enbs[i]}.");
                    else
                    {
                        var errorsString = String.Join(", ", exeptions.Select(e => e.Message));
                        console.AppendText($"\r\n[{DateTime.Now:T}] Произошли ошибки: {errorsString}. Завершение работы программы.");
                        break;
                    }
                }
                catch (Exception e)
                {
                    console.AppendText($"\r\n[{DateTime.Now:T}] Произошла ошибка {e.Message}. Завершение работы программы.");
                    break;
                }
                progressBar.Value = (int)((100d / parsedData.Enbs.Count) * i);
                currentEnb.Text = parsedData.Enbs[i].ToString();

            }
            System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{DateTime.Now.ToString("ddMMyy-hhmmss")} {mccString}-{mncString} EnbAllInfo.txt", JsonConvert.SerializeObject(results, Formatting.Indented));
            //System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{DateTime.Now.Hour}.{DateTime.Now.Minute} {mccString}-{mncString}" + "EnbNums.txt", JsonConvert.SerializeObject(successEnbsNums, Formatting.Indented));
            console.AppendText($"\n\t[{DateTime.Now:T}] Поиск сот окончен. Все найденные соты помещены в файлы EnbAllInfo.txt и EnbNums.txt.");



            //for (var cellId = minEnb; cellId <= maxEnb; cellId++)
            //{
            //    var GSMList = new List<CellInfo>();
            //    for (int i = 0; i < lacs.Count; i++)
            //    {
            //        for (int sectorIndex = 0; sectorIndex < sectors.Count; sectorIndex++)
            //        {
            //            GSMList.Add(new GSMCell() { countrycode = mcc, operatorid = mnc, cellid = cellId * 256 + sectors[sectorIndex], age = 1000, lac = lacs[i], signal_strength = -180 + sectorIndex });
            //        }
            //    }
            //    AllGSMList.Add(GSMList);
            return true;
        }

        public static void CancelTask()
        {
            if (ctSource != null)
                ctSource.Cancel();
        }
    }
}
