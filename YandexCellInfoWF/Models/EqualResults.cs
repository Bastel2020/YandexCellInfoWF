using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class EqualResults
    {
        public List<ResultsModel<DetailedInfoResults>> DetailResults { get; set; }
        public List<ResultsModel<BaseItemInfo>> FastSearchResults { get; set; }

        public EqualResults(List<ResultsModel<DetailedInfoResults>> detailResults, List<ResultsModel<BaseItemInfo>> allInfoResults)
        {
            DetailResults = detailResults;
            FastSearchResults = allInfoResults;
        }

        public BaseItemInfo ContainsValue(int enb, IEnumerable<int> sectors)
        {
            BaseItemInfo item = null;
            foreach(var detailResult in DetailResults)
            {
                var enbData = detailResult.Enbs.FirstOrDefault(dr => dr.)
            }
            var mathDetailResults = DetailResults.Where(dr => dr.Enbs.Any()
        }
    }
}
