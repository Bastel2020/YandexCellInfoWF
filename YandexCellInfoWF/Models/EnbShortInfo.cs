using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class EnbShortInfo
    {
        public int Enb;
        public float Latitude;
        public float Longitude;
        public float Precision;
        public string FormattedLocation { get
            {
                return $"{Latitude}, {Longitude}";
            }
            private set { }
        }

        public EnbShortInfo(int enb, float latitude, float longitude, float precision)
        {
            Enb = enb;
            Latitude = latitude;
            Longitude = longitude;
            Precision = precision;
        }

        public EnbShortInfo(int enb, YandexResponse yandexResponse)
        {
            Enb = enb;
            Latitude = yandexResponse.Latitude;
            Longitude = yandexResponse.Longitude;
            Precision = yandexResponse.Precision;
        }
    }
}
