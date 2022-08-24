using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models
{
    public class YandexResponse
    {
        [JsonProperty("latitude")]
        public float Latitude { get; set; }
        [JsonProperty("longitude")]
        public float Longitude { get; set; }
        [JsonProperty("altitude")]
        public float Altitude { get; set; }
        [JsonProperty("precision")]
        public float Precision { get; set; }
        [JsonProperty("altitude_precision")]
        public float AltitudePrecision { get; set; }
        [JsonProperty("type")]
        public string LocationType { get; set; }
    }
}
