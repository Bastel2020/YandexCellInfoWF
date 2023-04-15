using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using YandexCellInfoWF.Models;

namespace YandexCellInfoWF.Workers
{
    public static class InputParser
    {
        public static (bool Success, string Message) ParseInputWithoutSector(InputData input, out OutputData output)
        {
            output = new OutputData();

            if (!int.TryParse(input.MccString, out int mcc))
                return (false, "В поле MCC не число");
            output.Mcc = mcc;

            if (!int.TryParse(input.MncString, out int mnc))
                return (false, "В поле MNC не число");
            output.Mnc = mnc;

            try
            {
                var enbToAdd = new List<int>();
                var enbRawData = input.EnbsString;
                output.Enbs = enbRawData
                    .Split(new[] { ',', ' ', '\n', '\t', '\r', ';', ':', '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ParseRange(enbToAdd))
                    .ToList();
                output.Enbs.AddRange(enbToAdd);

                output.Enbs = output.Enbs
                    .Distinct()
                    .OrderBy(v => v)
                    .ToList();
            }
            catch (Exception e) { return (false, e.Message); }

            try
            {
                var lacsToAdd = new List<int>();
                var lacsRawData = input.LacsString;
                output.Lacs = lacsRawData
                    .Split(new[] { ',', ' ', '\n', '\t', '\r', ';', ':', '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ParseRange(lacsToAdd))
                    .ToList();
                output.Lacs
                    .AddRange(lacsToAdd);

                output.Lacs = output.Lacs
                    .Distinct()
                    .OrderBy(b => b)
                    .ToList();
            }
            catch(Exception e) { return (false, e.Message); }

            return (true, "");
        }

        public static (bool Success, string Message) ParseInputWithSector(InputData input, out OutputData output)
        {
            var parsingWithoutSectorStatus = ParseInputWithoutSector(input, out output);
            if (!parsingWithoutSectorStatus.Success)
                return parsingWithoutSectorStatus;

            try
            {
                var sectorsToAdd = new List<int>();
                var sectorsRawData = input.SectorsString;
                output.Sectors = sectorsRawData
                    .Split(new[] { ',', ' ', '\n', '\t', '\r', ';', ':', '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ParseRange(sectorsToAdd))
                    .ToList();
                output.Sectors.AddRange(sectorsToAdd);

                output.Sectors = output.Sectors
                    .Distinct()
                    .OrderBy(b => b)
                    .ToList();
            }
            catch (Exception e) { return (false, e.Message); }

            return (true, "");
        }

        private static Func<string, int> ParseRange(List<int> sectorsToAdd)
        {
            return input =>
            {
                if (input.Contains('*'))
                {
                    var positions = new List<int>();
                    for (int i = 0; i < input.Length; i++)
                    {
                        if (input[i] != '*')
                            continue;
                        positions.Add(i);
                    }
                    var results = new List<int>();
                    var valToAdd = Enumerable.Range(0, (int)Math.Pow(10, positions.Count))
                    .Select(v => v.ToString().PadLeft(positions.Count))
                    .ToList();
                    //for (int i = 1; i < 10; i++)
                    //{
                    //    for (int k = 1; k < positions.Count; k++)
                    //    {
                    //        var strToAdd = "".PadLeft(positions.Count, '0').ToArray();
                    //        strToAdd[k] = char.Parse(i.ToString());
                    //        valToAdd.Add(string.Join("", strToAdd));
                    //    }
                    //}

                    foreach (var val in valToAdd)
                    {
                        if (val.Equals("0") && positions[0] == 0)
                            continue;
                        var result = input.Replace('*', '0').ToArray();
                        for(int valIndx = 0; valIndx < val.Length; valIndx++)
                        {
                            result[positions[valIndx]] = val[valIndx];
                        }
                        results.Add(int.Parse(string.Join("", result)));
                    }
                    sectorsToAdd.AddRange(results);
                    return results.Count > 0 ? results[0] : 0;
                }
                if (!input.Contains('-'))
                    return int.Parse(input);
                else
                {
                    if (input.Contains('*'))
                        throw new Exception("Символ маски \"*\" не может сочетаться с символом диапазона \"-\"");
                    var bounds = input.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(b => int.Parse(b))
                    .ToArray();
                    if (bounds.Length != 2)
                        throw new Exception("Некорректное использование символа \"-\"");
                    sectorsToAdd.AddRange(Enumerable.Range(bounds[0] + 1, bounds[1] - bounds[0]));
                    return bounds[0];
                }
            };
        }
    }
}
