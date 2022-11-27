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
using YandexCellInfoWF.Services;

namespace YandexCellInfoWF.Workers
{
    public static class ManyInfoWorker
    {
        private static CancellationTokenSource ctSource;
        private static CancellationToken ct;

        private static HttpClient client = new HttpClient();

        public static async Task<bool> SearchEnbs(TextBox console, ProgressBar progressBar, Label currentEnb, string apiKey, string mccString, string mncString, string enbsString, string lacsString, string sectorsString, CheckBox dontSaveFiles)
        {
            ctSource = new CancellationTokenSource();
            ct = ctSource.Token;

            var successInfo = new bool[25];
            var multiplierBan = false;
            Func<float> successRate = new Func<float>(() => (float)successInfo.Where(v => v == true).Count() / successInfo.Length);

            console.Text = $"[{DateTime.Now:T}] Начат поиск всех БС по заданным параметрам.";

            var parsedData = new OutputData();
            var commonInfo = new YandexRequestCommonInfo(apiKey);
            var results = new List<BaseItemInfo>();

            if (!InputParser.ParseInputWithSector(new InputData(mccString, mncString, enbsString, lacsString, sectorsString), out parsedData))
            {
                console.AppendText("\r\nОшибка в входных данных. Завершение работы алгоритма.");
                return false;
            }

            for (var i = 0; i < parsedData.Enbs.Count; i++)
            {
                var multiplier = successRate() < 0.08 ? successRate() < 0.005 ? 20 : 5 : 2;
                if (i > successInfo.Length && i + multiplier < parsedData.Enbs.Count && successRate() < 0.35 && !multiplierBan)
                {
                    var c = successRate();
                    var multiCells = new List<CellInfo>();
                    foreach (var lac in parsedData.Lacs)
                    {
                        for(int k = i; k < i + multiplier; k++)
                            foreach (var sector in parsedData.Sectors)
                                multiCells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enbNumber: parsedData.Enbs[k], sector: sector));
                    }
                    var multiResponse = await RequestService.MakeRequest(console, commonInfo, multiCells, ct);
                    if (multiResponse == null)
                        break;
                    if (multiResponse.Equals(new BaseItemInfo()))
                    {
                        currentEnb.Text = parsedData.Enbs[i + multiplier].ToString();
                        progressBar.Value = (int)Math.Round((100d / parsedData.Enbs.Count) * (i + multiplier));
                        i += multiplier - 1;
                        for (int k = i; k < i + multiplier; k++)
                        {
                            successInfo[k % successInfo.Length] = false;
                        }
                        continue;
                    }
                    else
                        multiplierBan = true;
                }
                currentEnb.Text = parsedData.Enbs[i].ToString();
                progressBar.Value = (int)Math.Round((100d / parsedData.Enbs.Count) * i);

                var cells = new List<CellInfo>();
                foreach (var lac in parsedData.Lacs)
                {
                    foreach (var sector in parsedData.Sectors)
                        cells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enbNumber: parsedData.Enbs[i], sector: sector));
                }

                var response = await RequestService.MakeRequest(console, commonInfo, cells, ct);
                if (response == null)
                    break;
                if (!response.Equals(new BaseItemInfo()))
                {
                    successInfo[i % successInfo.Length] = true;
                    response.Number = parsedData.Enbs[i];
                    results.Add(response);
                    console.AppendText($"\r\n[{DateTime.Now:T}] Найдено! Enb: {parsedData.Enbs[i]}." +
                        $"\r\nGPS: {response.Latitude:0.00000}, {response.Longitude:0.00000}");
                    console.ScrollToCaret();
                    multiplierBan = false;
                }
                else
                {
                    successInfo[i % successInfo.Length] = false;
                }
            }
            if (results.Count == 0)
            {
                console.AppendText($"\n\t[{DateTime.Now:T}] Поиск сот окончен - нет найденых.");
                return true;
            }
            if (dontSaveFiles.Checked)
            {
                console.AppendText($"\n\t[{DateTime.Now:T}] Поиск сот окончен. Найдено: {results.Count}");
                return true;
            }
            System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{DateTime.Now.ToString("ddMMyy-hhmmss")} {mccString}-{mncString} EnbAllInfo.txt", JsonConvert.SerializeObject(results, Formatting.Indented));
            System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{DateTime.Now.ToString("ddMMyy-hhmmss")} {mccString}-{mncString} EnbNums.txt", JsonConvert.SerializeObject(results.Select(r => r.Number), Formatting.Indented));

            var kml = await KmlService.GetKMLAsync(results);
            System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{DateTime.Now.ToString("ddMMyy-hhmmss")} {mccString}-{mncString} map.kml", kml);

            console.AppendText($"\n\t[{DateTime.Now:T}] Поиск сот окончен - найдено: {results.Count}. Результаты помещены в файлы EnbAllInfo.txt и EnbNums.txt.");

            return true;
        }

        public static void CancelTask()
        {
            if (ctSource != null)
                ctSource.Cancel();
        }
    }
}
