using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YandexCellInfoWF.Models;

namespace YandexCellInfoWF.Workers
{
    public static class DetailedInfoWorker
    {
        private static CancellationTokenSource ctSource;
        private static CancellationToken ct;

        private static HttpClient client = new HttpClient();

        public static async Task<bool> SearchEnbs(TextBox console, ProgressBar progressBar, Label currentEnb, string apiKey, string mccString, string mncString, string enbsString, string lacsString, bool checkLac)
        {
            console.Text = $"[{DateTime.Now:T}] Начат подробный поиск БС";

            var result = await StartWorker(console, progressBar, currentEnb, apiKey, mccString, mncString, enbsString, lacsString, checkLac);
            if (result == null)
                return false;
            if (result.Count == 0)
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Подробный поиск БС завершен - нет найденых.");
                return true;
            }
            System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{DateTime.Now.ToString("ddMMyy-hhmmss")} {mccString}-{mncString} EnbDetailedInfo.txt", JsonConvert.SerializeObject(result, Formatting.Indented));
            console.AppendText($"\r\n[{DateTime.Now:T}] Подробный поиск БС завершен. Результаты сохранены в файл EnbDetailedInfo.txt.");
            return true;
        }

        private static async Task<List<EnbFullInfo>> StartWorker(TextBox console, ProgressBar progressBar, Label currentEnb, string apiKey, string mccString, string mncString, string enbsString, string lacsString, bool checkLac)
        {
            ctSource = new CancellationTokenSource();
            ct = ctSource.Token;

            var parsedData = new OutputData();
            var commonInfo = new YandexRequestCommonInfo(apiKey);
            var results = new List<EnbFullInfo>();
            var commonSectors = new List<int>();
            var otherSectors = Enumerable.Range(0, 256).ToList();


            if (!InputParser.ParseInputWithoutSector(new InputData(mccString, mncString, enbsString, lacsString), out parsedData))
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Ошибка в входных данных. Завершение работы алгоритма.");
                return null;
            }

            for (var i = 0; i < parsedData.Enbs.Count; i++)
            {
                console.AppendText($"\r\n[{DateTime.Now:T}] Анализ Enb: {parsedData.Enbs[i]}");

                currentEnb.Text = parsedData.Enbs[i].ToString();
                progressBar.Value = (int)Math.Round((100d / parsedData.Enbs.Count) * i);

                var cells = new List<CellInfo>();
                var currentEnbInfo = new EnbFullInfo(parsedData.Enbs[i]);

                if (commonSectors.Count > 0)
                {
                    var allCommonCells = new List<CellInfo>();
                    foreach (var sector in commonSectors)
                        foreach(var lac in parsedData.Lacs)
                        {
                            allCommonCells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enbNumber: parsedData.Enbs[i], sector: sector));
                        }
                    var checkAllCommonSectorsResult = await MakeRequest(console, commonInfo, allCommonCells);
                    if (!(checkAllCommonSectorsResult.Equals(new BaseItemInfo())))
                    {
                        foreach (var sector in commonSectors)
                        {
                            cells = new List<CellInfo>();

                            foreach (var lac in parsedData.Lacs)
                                cells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enbNumber: parsedData.Enbs[i], sector: sector));

                            var requestResult = await MakeRequest(console, commonInfo, cells, sector);

                            if (requestResult == null)
                                return results;

                            else if (!(requestResult.Equals(new BaseItemInfo())))
                            {
                                if (checkLac)
                                {
                                    var lacs = await DetectLacs(console, commonInfo, cells, sector);
                                    requestResult.lac = String.Join(", ", lacs);
                                }
                                    currentEnbInfo.AddSector(requestResult);
                            }
                        }
                    }
                }

                var sectorsCache = new List<List<int>> { otherSectors };
                while (sectorsCache.Count > 0)
                {
                    cells = new List<CellInfo>();
                    var currentSectors = sectorsCache.First();

                    foreach (var sector in currentSectors)
                    {
                        foreach (var lac in parsedData.Lacs)
                            cells.Add(new CellInfo(parsedData.Mcc, parsedData.Mnc, lac, enbNumber: parsedData.Enbs[i], sector: sector));
                    }
                    var requestResult = await MakeRequest(console, commonInfo, cells);

                    if (requestResult == null)
                        return results;

                    else if (!(requestResult.Equals(new BaseItemInfo())))
                    {
                        var firstSectorNum = currentSectors.First();
                        if (currentSectors.All(s => s == firstSectorNum))
                        {
                            requestResult.Number = firstSectorNum;
                            if (checkLac)
                            {
                                var lacs = await DetectLacs(console, commonInfo, cells, firstSectorNum);
                                requestResult.lac = String.Join(", ", lacs);
                            }
                            currentEnbInfo.AddSector(requestResult);

                            commonSectors.Add(firstSectorNum);
                            otherSectors.Remove(firstSectorNum);
                        }
                        else
                            sectorsCache.AddRange(Extensions.ChunkExtension.ToChunks(currentSectors, currentSectors.Count / 2));
                    }

                    sectorsCache.Remove(currentSectors);
                }
                if (currentEnbInfo.Sectors.Count > 0)
                {
                    currentEnbInfo.Sectors = currentEnbInfo.Sectors
                        .OrderBy(s => s.Number)
                        .ToList();
                    console.AppendText($"\r\nНайдены сектора: {String.Join(", ", currentEnbInfo.Sectors.Select(s => s.Number))}");
                    results.Add(currentEnbInfo);
                }
            }
            return results;
        }

        private static async Task<BaseItemInfo> MakeRequest(TextBox console, YandexRequestCommonInfo commonInfo, List<CellInfo> cells, int sectorNum = -1, bool repeated = false)
        {
            var request = JsonConvert.SerializeObject(new YandexRequest(commonInfo, cells));
            var content = new StringContent("json=" + request, Encoding.UTF8, "application/json");
            var parsedResponse = new YandexResponse();
            var result = new BaseItemInfo();
            try
            {
                await Task.Run(async () =>
                {
                    ct.ThrowIfCancellationRequested();

                    var response = await client.PostAsync("http://api.lbs.yandex.net/geolocation", content).Result.Content.ReadAsStringAsync();
                    parsedResponse = JObject.Parse(response)["position"].ToObject<YandexResponse>();
                }, ctSource.Token);
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
                    console.AppendText($"\r\n[{DateTime.Now:T}] Произошла ошибка {e.Message}. Пропуск сектора.");
                    return result;
                }
                console.AppendText($"\r\n[{DateTime.Now:T}] Произошла ошибка {e.Message}. Повтор.");
                return await MakeRequest(console, commonInfo, cells, sectorNum, true);
            }
        }

        private static async Task<List<int>> DetectLacs(TextBox console, YandexRequestCommonInfo commonInfo, List<CellInfo> cells, int sectorNum)
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
                    var response = await MakeRequest(console, commonInfo, sectorsCache.First(), sectorNum);

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
