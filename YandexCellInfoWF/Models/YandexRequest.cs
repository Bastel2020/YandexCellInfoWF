using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class YandexRequest
    {
        [JsonProperty("common")]
        public YandexRequestCommonInfo CommonInfo;
        [JsonProperty("gsm_cells")]
        public List<CellInfo> CellsList;
        public YandexRequest() { }
        public YandexRequest(YandexRequestCommonInfo commonInfo, List<CellInfo> cellList)
        {
            CommonInfo = commonInfo;
            CellsList = cellList;
        }
    }
}
