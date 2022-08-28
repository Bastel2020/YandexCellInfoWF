using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class EnbFullInfo : LocationInfo
    {
        public int Enb;
        public List<BaseItemInfo> Sectors
        {
            get { return _sectors; }
            set
            {
                _sectors = value;
                if (value.Count > 0)
                {
                    Longitude = value.Select(s => s.Longitude).Average();
                    Latitude = value.Select(s => s.Latitude).Average();
                }
            }
        }
        private List<BaseItemInfo>  _sectors;

        public EnbFullInfo() { Sectors = new List<BaseItemInfo>(); }
        public EnbFullInfo(int EnbNum)
        {
            Enb = EnbNum;
            Sectors = new List<BaseItemInfo>();
        }
        public EnbFullInfo(int EnbNum, List<BaseItemInfo> sectors)
        {
            Enb = EnbNum;
            Sectors = sectors;
        }

        public void AddSector(BaseItemInfo sector)
        {
            Sectors.Add(sector);
            Longitude = Sectors.Select(s => s.Longitude).Average();
            Latitude = Sectors.Select(s => s.Latitude).Average();
        }
    }
}
