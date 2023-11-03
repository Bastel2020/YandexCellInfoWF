//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using YandexCellInfoWF.Models;
//using YandexCellInfoWF.Models.Yandex;
//using YandexCellInfoWF.Services;

//namespace YandexCellInfoWF.Workers
//{
//    public static class ManyInfoWorker
//    {
//        private static CancellationTokenSource ctSource;
//        private static CancellationToken ct;

//        private static HttpClient client = new HttpClient();

//        public static async Task<bool> SearchEnbs(TextBox console, ProgressBar progressBar, Label currentEnb, Label totalFound, Label requestsTodayCount, string apiKey, string mccString, string mncString, string enbsString, string lacsString, string sectorsString, CheckBox dontSaveFiles)
//        {
//            ctSource = new CancellationTokenSource();
//            ct = ctSource.Token;

//            var maxMultiplier = 50;
//            var successInfo = new bool[15];
//            var multiplierBan = false;
//            var splitMultiplayer = false;
//            Func<decimal> successRate = new Func<decimal>(() => (decimal)successInfo.Where(v => v == true).Count() / successInfo.Length);

//            console.Text = $"[{DateTime.Now:T}] Начат поиск всех БС по заданным параметрам.";

//            var parsedData = new OutputData();
//            var commonInfo = new YandexRequestCommonInfo(apiKey);
//            var results = new List<BaseItemInfo>();
//            var successCounter = 0;

//            var inputValidationResult = InputParser.ParseInputWithSector(new InputData(mccString, mncString, enbsString, lacsString, sectorsString), out parsedData);
//            if (!inputValidationResult.Success)
//            {
//                console.AppendText($"\r\n[{DateTime.Now:T}] Ошибка в входных данных ({inputValidationResult.Message}). Завершение работы алгоритма.");
//                return false;
//            }

//            var existingResultsModel = SearcherService.GetAllEqualResults
//                (parsedData.Mcc, parsedData.Mnc, parsedData.Sectors.ToHashSet(), parsedData.Lacs.ToHashSet());
//            var existingEnbs = SearcherService.GenerateEnbsDictionary(existingResultsModel);
//            for (var i = 0; i < parsedData.Enbs.Count; i++)
//            {
//                var foundInLocalFile = false;
//                BaseItemInfo response = null;
//                if (!existingEnbs.ContainsKey(parsedData.Enbs[i]))
//                {
//                    var multiplier = GetMultiplier(successRate(), maxMultiplier);
//                    while (i > successInfo.Length && multiplier >= 2 && !multiplierBan)
//                    {
//                        if (i + multiplier >= parsedData.Enbs.Count)
//                        {
//                            multiplier = parsedData.Enbs.Count - i - 1;
//                            if (multiplier < 2)
//                                break;
//                        }
//                        List<int> enbsToSearch = new List<int>();
//                        int currentIndex = i;
//                        while (enbsToSearch.Count < multiplier || currentIndex < parsedData.Enbs.Count)
//                        {
//                            if (!existingEnbs.ContainsKey(parsedData.Enbs[currentIndex]))
//                                enbsToSearch.Add(parsedData.Enbs[currentIndex]);
//                            else
//                            {
//                                //TODO: add to results exsisting data
//                                results.Add(response);
//                                successCounter++;
//                                totalFound.Text = successCounter.ToString();
//                                console.AppendText($"\r\n[{DateTime.Now:T}] Найдено! Enb: {parsedData.Enbs[i]}." +
//                                    $"\r\nGPS: {response.Latitude:0.00000}, {response.Longitude:0.00000}");
//                                console.ScrollToCaret();
//                            }
//                        }
//                        BaseItemInfo multiResponse = await MakeMultiEnbRequest(console, requestsTodayCount, parsedData, commonInfo, enbsToSearch);
//                        //Операция отменена
//                        if (multiResponse == null)
//                            break;
//                        //Не найдено
//                        if (multiResponse.Equals(new BaseItemInfo()))
//                        {
//                            currentEnb.Text = parsedData.Enbs[i + multiplier].ToString();
//                            progressBar.Value = (int)Math.Round((100d / parsedData.Enbs.Count) * (i + multiplier));
//                            i += multiplier;
//                            for (int k = i; k < i + multiplier; k++)
//                            {
//                                successInfo[k % successInfo.Length] = false;
//                            }
//                            if (!splitMultiplayer)
//                                multiplier = GetMultiplier(successRate(), maxMultiplier);
//                            else //Если стоял SplitMultiplayer, то скоро будет найдена БС, подрежем множитель
//                                multiplier /= 2;
//                            continue;
//                        }
//                        else if (multiplier >= 10)
//                        {
//                            multiplier /= 2;
//                            splitMultiplayer = true;
//                        }
//                        else
//                            multiplierBan = true;
//                    }

//                    currentEnb.Text = parsedData.Enbs[i].ToString();
//                    progressBar.Value = (int)Math.Round((100d / parsedData.Enbs.Count) * i);

//                    var cells = new List<CellInfo>();
//                    foreach (var lac in parsedData.Lacs)
//                    {
//                        foreach (var sector in parsedData.Sectors)
//                            cells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enbNumber: parsedData.Enbs[i], sector: sector));
//                    }

//                    response = await RequestService.MakeRequest(console, commonInfo, requestsTodayCount, cells, ct);
//                }
//                else //Если нашли в локальных данных
//                {
//                    foundInLocalFile = true;
//                    response = existingEnbs[parsedData.Enbs[i]];
//                }

//                if (response == null)
//                    break;
//                if (!response.Equals(new BaseItemInfo()))
//                {
//                    successInfo[i % successInfo.Length] = true;
//                    response.Number = parsedData.Enbs[i];
//                    results.Add(response);
//                    successCounter++;
//                    totalFound.Text = successCounter.ToString();
//                    console.AppendText($"\r\n[{DateTime.Now:T}] " + (foundInLocalFile ? "Найден локально!" : "Найдено") + $" Enb: {parsedData.Enbs[i]}." +
//                        $"\r\nGPS: {response.Latitude:0.00000}, {response.Longitude:0.00000}");
//                    console.ScrollToCaret();
//                    multiplierBan = false;
//                    splitMultiplayer = false;
//                }
//                else
//                {
//                    successInfo[i % successInfo.Length] = false;
//                }
//            }
//            if (results.Count == 0)
//            {
//                console.AppendText($"\r\n[{DateTime.Now:T}] Поиск сот окончен - нет найденых.");
//                return true;
//            }
//            if (dontSaveFiles.Checked)
//            {
//                console.AppendText($"\r\n[{DateTime.Now:T}] Поиск сот окончен. Найдено: {results.Count}");
//                return true;
//            }
//            var dir = Environment.CurrentDirectory + $"\\{mccString}-{mncString}";
//            Directory.CreateDirectory(dir);
//            var preparedResults = new ResultsModel<BaseItemInfo>(mccString, mncString, enbsString, lacsString, sectorsString, results);
//            File.WriteAllText(dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} EnbAllInfo.txt", JsonConvert.SerializeObject(preparedResults, Formatting.Indented));
//            File.WriteAllText(path: dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} EnbNums.txt", JsonConvert.SerializeObject(results.Select(r => r.Number), Formatting.Indented));

//            var kml = await KmlService.GetKMLAsync(results);
//            File.WriteAllText(dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} map.kml", kml);

//            console.AppendText($"\r\n[{DateTime.Now:T}] Поиск сот окончен - найдено: {results.Count}. Результаты помещены в файлы EnbAllInfo.txt и EnbNums.txt.");

//            return true;
//        }

//        private static async Task<BaseItemInfo> MakeMultiEnbRequest(TextBox console, Label requestsTodayCount, OutputData parsedData, YandexRequestCommonInfo commonInfo, List<int> enbs)
//        {
//            var multiCells = new List<CellInfo>();
//            foreach (var lac in parsedData.Lacs)
//            {
//                foreach(var enb in enbs)
//                    foreach (var sector in parsedData.Sectors)
//                        multiCells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enb, sector: sector));
//            }
//            var multiResponse = await RequestService.MakeRequest(console, commonInfo, requestsTodayCount, multiCells, ct);
//            return multiResponse;
//        }

//        private static int GetMultiplier(decimal successRate, int maxMultiplier)
//        {
//            if (successRate > 0.4M)
//                return 1;
//            if (successRate > 0.25M)
//                return 2;
//            if (successRate > 0.15M)
//                return 3;
//            if (successRate > 0.10M)
//                return 5;
//            if (successRate > 0.015M)
//                return 10;
//            else
//                return maxMultiplier;
//        }

//        public static void CancelTask()
//        {
//            if (ctSource != null)
//                ctSource.Cancel();
//        }
//    }
//}
