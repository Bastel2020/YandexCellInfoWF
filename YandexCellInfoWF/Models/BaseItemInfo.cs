using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class BaseItemInfo : LocationInfo
    {
        public int Number { get; set; }
        public string LAC { get; set; }

        public BaseItemInfo(int num, float latitude, float longitude, float precision)
        {
            Number = num;
            Latitude = latitude;
            Longitude = longitude;
            Precision = precision;
        }

        public BaseItemInfo(int num, YandexResponse yandexResponse)
        {
            Number = num;
            Latitude = yandexResponse.Latitude;
            Longitude = yandexResponse.Longitude;
            Precision = yandexResponse.Precision;
        }

        public BaseItemInfo() { }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(BaseItemInfo))
                return false;
            var secondObj = obj as BaseItemInfo;
            return Number == secondObj.Number && Latitude == secondObj.Latitude && Longitude == secondObj.Longitude && Precision == secondObj.Precision;
        }
    }
}
