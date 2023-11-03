using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YandexCellInfoWF.Models;
using YandexCellInfoWF.Workers;

namespace YandexCellInfoWF.Services
{
    public static class SearcherService
    {
        public static EqualResults GetAllEqualResults(int mcc, int mnc, HashSet<int> sectors, HashSet<int> lacs)
        {
            var operatorFolder = Environment.CurrentDirectory + $"\\{mcc}-{mnc}";
            var detailFiles = Directory
                .GetFiles(operatorFolder)
                .Where(f => f.EndsWith(".txt") && f.Contains("EnbDetailedInfo") &&
                    CheckDate(f))
                .ToArray();

            var allInfoFiles = Directory
                .GetFiles(operatorFolder)
                .Where(f => f.EndsWith(".txt") && f.Contains("EnbAllInfo") &&
                CheckDate(f))
                .ToArray();

            var parsedDetailedFiles = new List<DetailedInfoResults>();
            var allInfoResults = new List<ResultsModel<BaseItemInfo>>();

            foreach (var file in detailFiles)
            {
                try
                {
                    var x = File.ReadAllText(file);
                    var parsedFile =
                        JsonConvert.DeserializeObject<DetailedInfoResults>(x);

                    if (!parsedFile.MCC.Equals(mcc.ToString()) || !parsedFile.MNC.Equals(mnc.ToString())
                        || !IsValidFile(parsedFile.LACs, lacs))
                        continue;
                    parsedDetailedFiles.Add(parsedFile);
                }
                catch { }
            }

            foreach (var file in allInfoFiles)
            {
                try
                {
                    var parsedFile =
                        JsonConvert.DeserializeObject<ResultsModel<BaseItemInfo>>(File.ReadAllText(file));

                    if (!parsedFile.MCC.Equals(mcc.ToString()) || !parsedFile.MNC.Equals(mnc.ToString())
                        || !IsValidFile(parsedFile.SearchRange, sectors)
                        || !IsValidFile(parsedFile.LACs, lacs)
                        || !IsValidFile(parsedFile.SectorsRange, sectors))
                        continue;
                    allInfoResults.Add(parsedFile);
                }
                catch { }
            }
            return new EqualResults(parsedDetailedFiles, allInfoResults);
        }

        public static Dictionary<int, BaseItemInfo> GenerateEnbsDictionary(EqualResults input, HashSet<int> enteredSectors)
        {
            var results = new Dictionary<int, BaseItemInfo>();
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            foreach(var fastResult in input.FastSearchResults)
            {
                foreach(var enb in fastResult.Enbs)
                {
                    if (!results.ContainsKey(enb.Number))
                        results.Add(enb.Number, enb);
                }
            }
            foreach (var detailResult in input.DetailResults)
            {
                foreach (var enb in detailResult.Enbs)
                {
                    var enbSectors = enb.Sectors.Select(s => s.Number)
                        .ToArray();
                    if (!enbSectors.Any(s => enteredSectors.Contains(s)))
                        continue;

                    if (!results.ContainsKey(enb.Enb))
                        results.Add(enb.Enb, new BaseItemInfo(enb.Enb, enb.Latitude, enb.Longitude, enb.Precision));
                    else //Приоритет детальным поискам
                        results[enb.Enb] = new BaseItemInfo(enb.Enb, enb.Latitude, enb.Longitude, enb.Precision);
                }
            }
            return results;
        }
 

        //public static List<BaseItemInfo> GetEnbsFromEqualResults(EqualResults results, HashSet<int> sectors, HashSet<int> lacs)
        //{
        //    var allEnbs = new Dictionary<int, BaseItemInfo>();
        //    foreach (var resultModel in results.FastSearchResults)
        //    {
        //        var lacsNums = InputParser.ParseStringInput(resultModel.LACs);
        //        var sectorsNums = InputParser.ParseStringInput(resultModel.SectorsRange);
        //        if (lacsNums.Any(lac => !lacs.Contains(lac)))
        //            continue;
        //        if (sectorsNums.Any(sector => !sectors.Contains(sector)))
        //            continue;
        //        foreach (var baseItem in resultModel.Enbs)
        //            if (!allEnbs.ContainsKey(baseItem.Number))
        //                allEnbs.Add(baseItem.Number, baseItem);
        //    }
        //    foreach (var resultModel in results.DetailResults)
        //    {
        //        var lacsNums = InputParser.ParseStringInput(resultModel.LACs);
        //        var sectorsNums = InputParser.ParseStringInput(resultModel.SectorsRange);
        //        if (lacsNums.Any(lac => !lacs.Contains(lac)))
        //            continue;
        //        if (sectorsNums.Any(sector => !sectors.Contains(sector)))
        //            continue;
        //        foreach (var baseItem in resultModel.Enbs)
        //            if (!allEnbs.ContainsKey(baseItem.Number))
        //                allEnbs.Add(baseItem.Number, baseItem);
        //    }
        //}

        private static bool IsValidFile(string fileInput, HashSet<int> enteredInput)
        {
            var parsedFileInput = InputParser
                .ParseStringInput(fileInput)
                .ToHashSet();
            if (parsedFileInput == null || parsedFileInput.Count == 0)
                return false;
            return enteredInput
                .All(enteredLAC => parsedFileInput.Contains(enteredLAC));
        }

        private static bool CheckDate(string filePath)
        {
            try
            {
                var ddMMYY = filePath.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries).Last();
                
                var day = int.Parse(ddMMYY.Substring(0, 2));
                var month = int.Parse(ddMMYY.Substring(2, 2));
                var year = int.Parse(ddMMYY.Substring(4, 2)) + (DateTime.Now.Year - (DateTime.Now.Year % 100));
                var date = new DateTime(year, month, day);
                return DateTime.Now.AddDays(-180) < date;
            }
            catch { return false; }
        }
    }
}