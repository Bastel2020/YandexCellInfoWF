using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YandexCellInfoWF.Models;

namespace YandexCellInfoWF.Workers
{
    public static class InputParser
    {
        public static bool ParseInputWithoutSector(InputData input, out OutputData output)
        {
            output = new OutputData();

            if (!int.TryParse(input.MccString, out int mcc))
                return false;
            output.Mcc = mcc;

            if (!int.TryParse(input.MncString, out int mnc))
                return false;
            output.Mnc = mnc;

            try
            {
                var enbToAdd = new List<int>();
                var enbRawData = input.EnbsString;
                output.Enbs = enbRawData
                    .Split(new[] { ',', ' ', '\n', '\t', '\r', ';', ':', '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(rd =>
                    {
                        if (!rd.Contains('-'))
                            return int.Parse(rd);
                        else
                        {
                            var bounds = rd.Split('-')
                            .Select(b => int.Parse(b))
                            .ToArray();
                            if (bounds.Length != 2)
                                return 0;
                            enbToAdd.AddRange(Enumerable.Range(bounds[0] + 1, bounds[1] - bounds[0]));
                            return bounds[0];
                        }
                    })
                    .ToList();
                output.Enbs.AddRange(enbToAdd);
            }
            catch { return false; }

            try
            {
                var lacsToAdd = new List<int>();
                var lacsRawData = input.LacsString;
                output.Lacs = lacsRawData
                    .Split(new[] { ',', ' ', '\n', '\t', '\r', ';', ':', '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(rd =>
                    {
                        if (!rd.Contains('-'))
                            return int.Parse(rd);
                        else
                        {
                            var bounds = rd.Split('-')
                            .Select(b => int.Parse(b))
                            .ToArray();
                            if (bounds.Length != 2)
                                return 0;
                            lacsToAdd.AddRange(Enumerable.Range(bounds[0] + 1, bounds[1] - bounds[0]));
                            return bounds[0];
                        }
                    })
                    .ToList();
                output.Lacs.AddRange(lacsToAdd);
            }
            catch { return false; }

            return true;
        }

        public static bool ParseInputWithSector(InputData input, out OutputData output)
        {
            output = new OutputData();
            if (!ParseInputWithoutSector(input, out OutputData output1))
            {
                output = output1;
                return false;
            }
            output = output1;
            try
            {
                var sectorsToAdd = new List<int>();
                var sectorsRawData = input.SectorsString;
                output.Sectors = sectorsRawData
                    .Split(new[] { ',', ' ', '\n', '\t', '\r', ';', ':', '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(rd =>
                    {
                        if (!rd.Contains('-'))
                            return int.Parse(rd);
                        else
                        {
                            var bounds = rd.Split('-')
                            .Select(b => int.Parse(b))
                            .ToArray();
                            if (bounds.Length != 2)
                                return 0;
                            sectorsToAdd.AddRange(Enumerable.Range(bounds[0] + 1, bounds[1] - bounds[0]));
                            return bounds[0];
                        }
                    })
                    .ToList();
                output.Sectors.AddRange(sectorsToAdd);
            }
            catch { return false; }

            return true;
        }
    }
}
