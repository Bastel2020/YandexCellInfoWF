using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class DetailedInfoResults
    {
        public IEnumerable<EnbFullInfo> Enbs { get; set; }
        public string MCC { get; set; }
        public string MNC { get; set; }
        public string SearchRange { get; set; }
        public string LACs { get; set; }
        public DateTime SearchDate { get; set; }
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
                        .Select(k => $"{k.Key}: {k.Count()}");
                }
                catch { return null; }
            }
        }

        public DetailedInfoResults(string MCC, string MNC, string enbRange, string LACsRange, IEnumerable<EnbFullInfo> enbs)
        {
            Enbs = enbs;
            this.MCC = MCC;
            this.MNC = MNC;
            SearchRange = enbRange;
            LACs = LACsRange;
            SearchDate = DateTime.Now;
        }
    }
}