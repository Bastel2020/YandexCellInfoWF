using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace YandexCellInfoWF.Models
{
    public class YandexRequestCommonInfo
    {
        [JsonProperty("version")]
        public string Version = "1.0";
        [JsonProperty("api_key")]
        public string ApiKey; //https://yandex.ru/dev/locator/keys/get/
        public YandexRequestCommonInfo(string apiKey)
        {
            ApiKey = apiKey;
        }
    }
}
