using SharpKml.Base;
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
                
                if (value.Count > 0)
                {
                    var lonValues = value.Select(s => s.Longitude);
                    var lonMedian = Helpers.Math.GetMedian(lonValues);
                    Longitude = lonValues.Where(v => Math.Abs(v - lonMedian) < 0.3d).Average();

                    var latValues = value.Select(s => s.Latitude);
                    var latMedian = Helpers.Math.GetMedian(latValues);
                    Latitude = latValues.Where(v => Math.Abs(v - latMedian) < 0.3d).Average();
                }
                _sectors = value.Select(v =>
                {
                    if (Math.Abs(v.Latitude - Latitude) > 0.3)
                        v.Latitude = Latitude;
                    if (Math.Abs(v.Longitude - Longitude) > 0.3)
                        v.Longitude = Longitude;
                    return v;
                }).ToList();
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
            if (Precision == 0)
                Precision = sector.Precision;
            else
                Precision = Math.Min(Precision, sector.Precision);
        }
    }
}
