using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class ResultsModel<TInput>
    {
        public IEnumerable<TInput> Enbs { get; set; }
        public string MCC { get; set; }
        public string MNC { get; set; }
        public string SearchRange { get; set; }
        public string LACs { get; set; }
        public DateTime SearchDateUtc { get; set; }
        public int ResultHash { get; set; }

        public ResultsModel(string MCC, string MNC, string enbRange, string LACsRange, IEnumerable<TInput> enbs)
        {
            Enbs = enbs;
            this.MCC = MCC;
            this.MNC = MNC;
            SearchRange = enbRange;
            LACs = LACsRange;
            SearchDateUtc = DateTime.UtcNow;
            ResultHash = GetHashCode();
        }

        public override int GetHashCode()
        {
            var enbsHash = string.Join("", Enbs.Select(v => v.GetHashCode())).GetHashCode();
            return $"{enbsHash};{MCC};{MNC};{SearchRange};{LACs};{SearchDateUtc}".GetHashCode();
        }
    }
}