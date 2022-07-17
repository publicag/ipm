using Newtonsoft.Json;
using System.Collections.Generic;

namespace NBPHttp.ExchangeRateModel
{
    public class NBPExchangeRateHistory
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("rates")]
        public ICollection<NBPExchangeRateWithEffectiveDate> Rates { get; set; }

        public static NBPExchangeRateHistory DeserializeNBPExchangeRateHistory(string json)
        {
            return JsonConvert.DeserializeObject<NBPExchangeRateHistory>(json);
        }
    }
}
