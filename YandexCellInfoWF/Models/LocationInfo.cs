using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class LocationInfo
    {
        public float Latitude;
        public float Longitude;
        public float Precision;
        public string FormattedLocation
        {
            get
            {
                return $"{Latitude}, {Longitude}";
            }
            private set { }
        }
    }
}
