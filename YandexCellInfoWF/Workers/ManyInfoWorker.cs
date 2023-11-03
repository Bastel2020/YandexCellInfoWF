using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YandexCellInfoWF.Models;
using YandexCellInfoWF.Models.Yandex;
using YandexCellInfoWF.Services;

namespace YandexCellInfoWF.Workers
{
    public static class ManyInfoWorker
    {
        private static CancellationTokenSource ctSource;
        private static CancellationToken ct;

        public static async Task<bool> SearchEnbs(TextBox console, ProgressBar progressBar, Label currentEnb, Label totalFound, Label requestsTodayCount,
            string apiKey, string mccString, string mncString, string enbsString, string lacsString, string sectorsString, CheckBox dontSaveFiles)
        {
            ctSource = new CancellationTokenSource();
            ct = ctSource.Token;

            var maxMultiplier = 50;
            var successInfo = new bool[15];
            var multiplierBan = false;
            Func<decimal> successRate = new Func<decimal>(() => (decimal)successInfo.Where(v => v == true).Count() / successInfo.Length);

            console.Text = $"[{DateTime.Now:T}] Начат поиск всех БС по заданным параметрам.";

            var commonInfo = new YandexRequestCommonInfo(apiKey);
            var results = new List<BaseItemInfo>();
            var successCounter = 0;

            var inputValidationResult = InputParser.ParseInputWithSector(new InputData(mccString, mncString, enbsString, lacsString, sectorsString),
                out var parsedData);
            if (!inputValidationResult.Success)
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Ошибка в входных данных ({inputValidationResult.Message}). Завершение работы.");
                return false;
            }

            var hashsetSectors = parsedData.Sectors.ToHashSet();

            var existingResultsModel = SearcherService.GetAllEqualResults
                (parsedData.Mcc, parsedData.Mnc, hashsetSectors, parsedData.Lacs.ToHashSet());
            var existingEnbs = SearcherService.GenerateEnbsDictionary(existingResultsModel, hashsetSectors);

            existingResultsModel = null;
            hashsetSectors = null;

            var enbToRequest = parsedData.Enbs
                .Where(enb => !existingEnbs.ContainsKey(enb))
                .OrderBy(enb => enb)
                .ToArray();

            for (var i = 0; i < enbToRequest.Length; i++)
            {
                var localFoundEnb = existingEnbs.Where(enb =>
                {
                    if (i == 0)
                        return enb.Key < enbToRequest[0];
                    if (i == enbToRequest.Length - 1)
                        return enb.Key > enbToRequest[enbToRequest.Length - 1];
                    else
                        return enb.Key > enbToRequest[i-1] && enb.Key < enbToRequest[i];
                })
                    .Select(enb => enb.Value)
                    .ToArray();

                if (localFoundEnb.Length > 0)
                {
                    results.AddRange(localFoundEnb);
                    successCounter += localFoundEnb.Length;
                    totalFound.Text = successCounter.ToString();
                    multiplierBan = false;
                    foreach (var item in localFoundEnb)
                    {
                        console.AppendText($"\r\n[{DateTime.Now:T}] Найдено!* Enb: {item.Number}." +
                            $"\r\nGPS: {item.Latitude:0.00000}, {item.Longitude:0.00000}");
                        console.ScrollToCaret();
                    }
                }

                BaseItemInfo response = null;

                var multiplier = GetMultiplier(successRate(), maxMultiplier);
                //Следим, чтобы множитель не улетел за пределы массива
            
                while (i > successInfo.Length && multiplier >= 2 && !multiplierBan)
                {
                    multiplier = Math.Min(multiplier, enbToRequest.Length - i - 1);
                    BaseItemInfo multiResponse = await MakeMultiEnbRequest(console, requestsTodayCount, parsedData, commonInfo,
                        enbToRequest.Skip(i).Take(multiplier).ToArray());
                    //Операция отменена
                    if (multiResponse == null)
                        break;
                    //Не найдено
                    if (multiResponse.Equals(new BaseItemInfo()))
                    {
                        currentEnb.Text = enbToRequest[i + multiplier].ToString();
                        progressBar.Value = (int)Math.Round(100d / enbToRequest.Length * (i + multiplier));
                        i += multiplier;
                        for (int k = i; k < i + multiplier; k++)
                        {
                            successInfo[k % successInfo.Length] = false;
                        }
                        multiplier = GetMultiplier(successRate(), maxMultiplier);
                    }
                    else if (multiplier >= 4)
                    {
                        multiplier /= 2;
                    }
                    else
                        multiplierBan = true;
                }

                currentEnb.Text = enbToRequest[i].ToString();
                progressBar.Value = (int)Math.Round((100d / enbToRequest.Length) * i);

                var cells = new List<CellInfo>();
                foreach (var lac in parsedData.Lacs)
                {
                    foreach (var sector in parsedData.Sectors)
                        cells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enbNumber: enbToRequest[i], sector: sector));
                }

                response = await RequestService.MakeRequest(console, commonInfo, requestsTodayCount, cells, ct);
                
                //Операция отменена
                if (response == null)
                    break;
                if (!response.Equals(new BaseItemInfo()))
                {
                    successInfo[i % successInfo.Length] = true;
                    response.Number = enbToRequest[i];
                    results.Add(response);
                    successCounter++;
                    totalFound.Text = successCounter.ToString();
                    console.AppendText($"\r\n[{DateTime.Now:T}] Найдено! Enb: {enbToRequest[i]}." +
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
                console.AppendText($"\r\n[{DateTime.Now:T}] Поиск окончен - нет найденных.");
                return true;
            }
            if (dontSaveFiles.Checked)
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Поиск окончен. Найдено: {results.Count}");
                return true;
            }
            var dir = Environment.CurrentDirectory + $"\\{mccString}-{mncString}";
            Directory.CreateDirectory(dir);
            var preparedResults = new ResultsModel<BaseItemInfo>
                (mccString, mncString, enbsString, lacsString, sectorsString, results);
            File.WriteAllText(dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} EnbAllInfo.txt",
                JsonConvert.SerializeObject(preparedResults, Formatting.Indented));
            File.WriteAllText(path: dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} EnbNums.txt",
                JsonConvert.SerializeObject(results.Select(r => r.Number), Formatting.Indented));

            var kml = await KmlService.GetKMLAsync(results);
            File.WriteAllText(dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} map.kml", kml);

            console.AppendText($"\r\n[{DateTime.Now:T}] Поиск окончен - найдено: {results.Count}. Результаты в файлах EnbAllInfo.txt и EnbNums.txt.");

            return true;
        }

        private static async Task<BaseItemInfo> MakeMultiEnbRequest(TextBox console, Label requestsTodayCount, OutputData parsedData, YandexRequestCommonInfo commonInfo, IEnumerable<int> enbs)
        {
            var multiCells = new List<CellInfo>();
            foreach (var lac in parsedData.Lacs)
            {
                foreach(var enb in enbs)
                    foreach (var sector in parsedData.Sectors)
                        multiCells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enb, sector: sector));
            }
            var multiResponse = await RequestService.MakeRequest(console, commonInfo, requestsTodayCount, multiCells, ct);
            return multiResponse;
        }

        private static int GetMultiplier(decimal successRate, int maxMultiplier)
        {
            if (successRate > 0.4M)
                return 1;
            if (successRate > 0.25M)
                return 2;
            if (successRate > 0.15M)
                return 3;
            if (successRate > 0.10M)
                return 5;
            if (successRate > 0.015M)
                return 10;
            else
                return maxMultiplier;
        }

        public static void CancelTask()
        {
            ctSource?.Cancel();
        }
    }
}
