using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class CellInfo
    {
        [JsonProperty("countrycode")]
        public int MCC { get; set; }
        [JsonProperty("operatorid")]
        public int MNC { get; set; }
        [JsonProperty("cellid")]
        public int CellId { get; set; }
        [JsonProperty("lac")]
        public int LAC { get; set; }
        [JsonProperty("signal_strength")]
        public int SignalStrength { get; set; }
        [JsonProperty("age")]
        public int AgeInfo { get; set; }
        [JsonIgnore]
        public int Sector { get; set; }
        [JsonIgnore]
        public int EnbNumber { get; set; }

        public CellInfo(int mcc, int mnc, int lac, int cellId, int signalStrength = -45, int ageInfo = 1000)
        {
            MCC = mcc;
            MNC = mnc;
            LAC = lac;
            CellId = cellId;
            SignalStrength = signalStrength;
            AgeInfo = ageInfo;
            Sector = CellId % 256;
            EnbNumber = CellId / 256;
        }

        public CellInfo(int mcc, int mnc, int lac, int enbNumber, int sector, int signalStrength = -45, int ageInfo = 1000)
        {
            MCC = mcc;
            MNC = mnc;
            LAC = lac;
            EnbNumber = enbNumber;
            Sector = sector;
            CellId = EnbNumber * 256 + Sector;
            SignalStrength = signalStrength;
            AgeInfo = ageInfo;
        }
    }
}
