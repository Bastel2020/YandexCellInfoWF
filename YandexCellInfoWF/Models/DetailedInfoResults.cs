using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class DetailedInfoResults : ResultsModel<EnbFullInfo>
    {
        public IEnumerable<string> SectorsCount
        {
            get
            {
                try
                {
                    return Enbs
                        .SelectMany(e => e.Sectors.Select(s => s.Number))
                        .GroupBy(num => num)
                        .OrderBy(num => num.Key)
                        .Select(k => $"{k.Key}: {k.Count()}")
                        .ToArray();
                }
                catch { return null; }
            }
        }

        public DetailedInfoResults(string MCC, string MNC, string enbRange, string LACsRange, string sectorsRange, IEnumerable<EnbFullInfo> enbs) :
            base(MCC, MNC, enbRange, LACsRange, sectorsRange, enbs)
        {

        }
    }
}