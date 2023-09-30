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
                    CheckDate(f.Substring(0, 6)));

            var allInfoFiles = Directory
                .GetFiles(operatorFolder)
                .Where(f => f.EndsWith(".txt") && f.Contains("EnbAllInfo") &&
                CheckDate(f.Substring(0, 6)));

            var parsedDetailedFiles = new List<ResultsModel<DetailedInfoResults>>();
            var allInfoResults = new List<ResultsModel<BaseItemInfo>>();

            foreach (var file in detailFiles)
            { 
                try
                {
                    var parsedFile = 
                        JsonConvert.DeserializeObject<ResultsModel<DetailedInfoResults>>(File.ReadAllText(file));
                    
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

        public static List<BaseItemInfo> GetEnbsFromEqualResults(EqualResults results, HashSet<int> sectors, HashSet<int> lacs)
        {
            var allEnbs = new Dictionary<int, BaseItemInfo>();
            foreach (var resultModel in results.FastSearchResults)
            {
                var lacsNums = InputParser.ParseStringInput(resultModel.LACs);
                var sectorsNums = InputParser.ParseStringInput(resultModel.SectorsRange);
                if (lacsNums.Any(lac => !lacs.Contains(lac)))
                    continue;
                if (sectorsNums.Any(sector => !sectors.Contains(sector)))
                    continue;
                foreach (var baseItem in resultModel.Enbs)
                    if (!allEnbs.ContainsKey(baseItem.Number))
                        allEnbs.Add(baseItem.Number, baseItem);
            }
            foreach (var resultModel in results.DetailResults)
            {
                var lacsNums = InputParser.ParseStringInput(resultModel.LACs);
                var sectorsNums = InputParser.ParseStringInput(resultModel.SectorsRange);
                if (lacsNums.Any(lac => !lacs.Contains(lac)))
                    continue;
                if (sectorsNums.Any(sector => !sectors.Contains(sector)))
                    continue;
                foreach (var baseItem in resultModel.Enbs)
                    if (!allEnbs.ContainsKey(baseItem.Number))
                        allEnbs.Add(baseItem.Number, baseItem);
            }
        }

        private static bool IsValidFile(string fileInput, HashSet<int> enteredInput)
        {
            var parsedFileInput = InputParser
                .ParseStringInput(fileInput);
            if (parsedFileInput == null || parsedFileInput.Count == 0)
                return false;
            return parsedFileInput
                .All(pfi => enteredInput.Contains(pfi));
        }

        private static bool CheckDate(string ddMMYY)
        {
            try
            {
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
