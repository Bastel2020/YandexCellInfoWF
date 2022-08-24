using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class OutputData
    {
        public int Mcc { get; set; }
        public int Mnc { get; set; }
        public List<int> Enbs { get; set; }
        public List<int> Lacs { get; set; }
        public List<int> Sectors { get; set; }
        public OutputData() { }
        public OutputData(int mcc, int mnc, List<int> enbs, List<int> lacs, List<int> sectors)
        {
            Mcc = mcc;
            Mnc = mnc;
            Enbs = enbs;
            Lacs = lacs;
            Sectors = sectors;
        }
    }
}
