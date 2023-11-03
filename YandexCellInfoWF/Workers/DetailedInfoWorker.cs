using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    public static class DetailedInfoWorker
    {
        private static CancellationTokenSource ctSource;
        private static CancellationToken ct;

        private static List<int> commonSectors;
        private static List<int> allSectors = Enumerable.Range(0, 256).ToList();

        public static async Task<bool> SearchEnbs(TextBox console, ProgressBar progressBar, Label currentEnb, Label successCount, Label requsetsTodayCount, string apiKey, string mccString, string mncString, string enbsString, string lacsString, bool checkLac, CheckBox dontSaveFiles)
        {
            console.Text = $"[{DateTime.Now:T}] Начат подробный поиск БС";

            commonSectors = new List<int>();

            var result = await StartWorker(console, progressBar, currentEnb, successCount, requsetsTodayCount, apiKey, mccString, mncString, enbsString, lacsString, checkLac);
            if (result == null)
                return false;
            if (result.Count == 0)
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Подробный поиск БС завершен - нет найденых.");
                return true;
            }
            if (dontSaveFiles.Checked)
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Подробный поиск БС завершен, найдено БС: {result.Count}. Секторов: {result.SelectMany(r => r.Sectors).Count()}");
                return true;
            }
            var preparedResults = new DetailedInfoResults(mccString, mncString, enbsString, lacsString, "0-255", result);

            var dir = Environment.CurrentDirectory + $"\\{mccString}-{mncString}";
            Directory.CreateDirectory(dir);
            File.WriteAllText(dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} map.kml", await KmlService.GetKMLAsync(preparedResults.Enbs));
            File.WriteAllText(dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} EnbDetailedInfo.txt", JsonConvert.SerializeObject(preparedResults, Formatting.Indented));
            File.WriteAllText(dir + "\\" + $"{DateTime.Now:ddMMyy-hhmmss} {mccString}-{mncString} EnbNums.txt", JsonConvert.SerializeObject(preparedResults.Enbs.Select(e => e.Enb), Formatting.Indented));
            console.AppendText($"\r\n[{DateTime.Now:T}] Подробный поиск БС завершен, найдено БС: {result.Count}. Секторов: {result.SelectMany(r => r.Sectors).Count()} Результаты сохранены в файл EnbDetailedInfo.txt.");
            return true;
        }

        private static async Task<List<EnbFullInfo>> StartWorker(TextBox console, ProgressBar progressBar, Label currentEnb, Label successCount, Label requsetsTodayCount, string apiKey, string mccString, string mncString, string enbsString, string lacsString, bool checkLac)
        {
            ctSource = new CancellationTokenSource();
            ct = ctSource.Token;

            var parsedData = new OutputData();
            var commonInfo = new YandexRequestCommonInfo(apiKey);
            var results = new List<EnbFullInfo>();
            var successCounter = 0;

            var parseResult = InputParser.ParseInputWithoutSector(new InputData(mccString, mncString, enbsString, lacsString), out parsedData);
            if (!parseResult.Success)
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Ошибка в входных данных ({parseResult.Message}). Завершение работы алгоритма.");
                return null;
            }

            for (var i = 0; i < parsedData.Enbs.Count; i++)
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Анализ Enb: {parsedData.Enbs[i]}");

                currentEnb.Text = parsedData.Enbs[i].ToString();
                progressBar.Value = (int)Math.Round(100d / parsedData.Enbs.Count * i);

                var cells = new List<CellInfo>();
                var currentEnbInfo = new EnbFullInfo(parsedData.Enbs[i]);

                var existingResult = await FoundSectorsByChunks(console, commonInfo, requsetsTodayCount, allSectors, parsedData.Lacs, parsedData.Mcc, parsedData.Mnc, parsedData.Enbs[i], checkLac);
                if (existingResult == null)
                    return results;

                if (existingResult.Count == 0)
                    continue;

                var popularSectorsResult = await FoundSectorsByChunks(console, commonInfo, requsetsTodayCount, allSectors, parsedData.Lacs, parsedData.Mcc, parsedData.Mnc, parsedData.Enbs[i], checkLac);
                if (popularSectorsResult == null)
                    return results;
                currentEnbInfo.Sectors.AddRange(popularSectorsResult);

                currentEnbInfo.Sectors = currentEnbInfo.Sectors
                    .OrderBy(s => s.Number)
                    .ToList();
                console.AppendText($"\r\nНайдены сектора: {string.Join(", ", currentEnbInfo.Sectors.Select(s => s.Number))}");

                results.Add(currentEnbInfo);

                successCounter++;
                successCount.Text = successCounter.ToString();
            }
            return results;
        }

        private static async Task<List<BaseItemInfo>> FoundSectorsByChunks(TextBox console, YandexRequestCommonInfo commonInfo, Label requsetsTodayCount, IEnumerable<int> sectors, IEnumerable<int> lacs, int mcc, int mnc, int enbId, bool checkLac)
        {
            var results = new List<BaseItemInfo>();

            var sectorsCache = new List<List<int>> { sectors.ToList() };

            while (sectorsCache.Count > 0)
            {
                var bypassChunk = sectorsCache.First();
                var currentChunk = sectorsCache.First();
                var cells = new List<CellInfo>();

                if (commonSectors.Count > 6 && currentChunk.Count <= 16)
                {
                    var intersecs = currentChunk.Intersect(commonSectors).ToList();
                    if (intersecs.Count > 0)
                    {
                        foreach (var commonSector in intersecs)
                        {
                            var tempCells = new List<CellInfo>();
                            foreach (var lac in lacs)
                                tempCells.Add(new CellInfo(mcc, mnc, lac, enbNumber: enbId, commonSector));

                            var commonRequestResult = await RequestService.MakeRequest(console, commonInfo, requsetsTodayCount, tempCells, ct);

                            if (commonRequestResult == null) //cancel throws
                                return null;
                            else if (!(commonRequestResult.Equals(new BaseItemInfo())))
                            {
                                commonRequestResult.Number = commonSector;
                                if (checkLac)
                                {
                                    var detectedLacs = await DetectLacs(console, commonInfo, requsetsTodayCount, tempCells, commonSector);
                                    commonRequestResult.lac = string.Join(", ", lacs);
                                }
                                results.Add(commonRequestResult);
                            }
                        }
                    }
                    currentChunk = currentChunk.Except(intersecs).ToList();
                }


                foreach (var sector in currentChunk)
                {
                    foreach (var lac in lacs)
                        cells.Add(new CellInfo(mcc, mnc, lac, enbNumber: enbId, sector: sector));
                }
                var requestResult = await RequestService.MakeRequest(console, commonInfo, requsetsTodayCount, cells, ct);

                if (requestResult == null) //cancel throws
                    return null;

                else if (!(requestResult.Equals(new BaseItemInfo())))
                {
                    if (currentChunk.Count == 1)
                    {
                        requestResult.Number = currentChunk[0];
                        if (checkLac)
                        {
                            var detectedLacs = await DetectLacs(console, commonInfo, requsetsTodayCount, cells, currentChunk[0]);
                            requestResult.lac = string.Join(", ", lacs);
                        }
                        results.Add(requestResult);

                        if (!commonSectors.Contains(currentChunk[0]))
                        {
                            commonSectors.Add(currentChunk[0]);
                            commonSectors = commonSectors
                                .OrderBy(cs => cs)
                                .ToList();
                        }
                    }
                    else
                        sectorsCache.AddRange(Extensions.ChunkExtension.ToChunks(currentChunk, (int)Math.Ceiling(currentChunk.Count / 2d)));
                }
                sectorsCache.Remove(bypassChunk);
            }

            return results;
        }

            private static async Task<List<int>> DetectLacs(TextBox console, YandexRequestCommonInfo commonInfo, Label requsetsTodayCount, List<CellInfo> cells, int sectorNum)
        {
            var result = new List<int>();
            await Task.Run(async () =>
            {

                var firstCellSectorNum = cells.First().Sector;
                if (!cells.All(c => c.Sector == firstCellSectorNum))
                    throw new ArgumentException("Ошибка при определении LAC: не все сектора-кандидаты имеют одинаковый номер.");

                var sectorsCache = new List<List<CellInfo>>();
                sectorsCache.AddRange(Extensions.ChunkExtension.ToChunks(cells, cells.Count / 2).ToList());

                while (sectorsCache.Count > 0)
                {
                    var current = sectorsCache.First();
                    var response = await Services.RequestService.MakeRequest(console, commonInfo, requsetsTodayCount, sectorsCache.First(), ct, sectorNum);

                    if (response == null)
                        return;

                    if (response.Equals(new BaseItemInfo()))
                        sectorsCache.Remove(current);
                    else
                    {
                        if (current.Count == 1)
                            result.Add(current.First().LAC);
                        else
                            sectorsCache.AddRange(Extensions.ChunkExtension.ToChunks(current, current.Count / 2).ToList());
                        sectorsCache.Remove(current);
                    }
                }
            });
            return result;
        }

        public static void CancelTask()
        {
            if (ctSource != null)
                ctSource.Cancel();
        }
    }
}
