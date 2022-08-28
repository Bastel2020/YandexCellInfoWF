using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class InputData
    {
        public string MccString { get; set; }
        public string MncString { get; set; }
        public string EnbsString { get; set; }
        public string LacsString { get; set; }
        public string SectorsString { get; set; }
        public InputData(string mccString, string mncString, string enbsString, string lacsString, string sectorsString)
        {
            MccString = mccString;
            MncString = mncString;
            EnbsString = enbsString;
            LacsString = lacsString;
            SectorsString = sectorsString;
        }
        public InputData(string mccString, string mncString, string enbsString, string lacsString)
        {
            MccString = mccString;
            MncString = mncString;
            EnbsString = enbsString;
            LacsString = lacsString;
        }
    }
}
