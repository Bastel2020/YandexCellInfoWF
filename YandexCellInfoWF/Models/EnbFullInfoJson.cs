using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class EnbFullInfoJson : LocationInfo
    {
        public int Enb;
        public List<BaseItemInfo> Sectors { get; set; }

        public EnbFullInfoJson() { Sectors = new List<BaseItemInfo>(); }
        public EnbFullInfoJson(int EnbNum)
        {
            Enb = EnbNum;
            Sectors = new List<BaseItemInfo>();
        }
        public EnbFullInfoJson(int EnbNum, List<BaseItemInfo> sectors)
        {
            Enb = EnbNum;
            Sectors = sectors;
        }

        public void AddSector(BaseItemInfo sector)
        {
            Sectors.Add(sector);
            Longitude = Sectors.Select(s => s.Longitude).Average();
            Latitude = Sectors.Select(s => s.Latitude).Average();
            if (Precision == 0)
                Precision = sector.Precision;
            else
                Precision = Math.Min(Precision, sector.Precision);
        }
    }
}
