using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NBPHttp.ExchangeRateModel
{
    public class NBPExchangeRateResponse
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("no")]
        public string No { get; set; }
        [JsonProperty("effectiveDate")]
        public string EffectiveDate { get; set; }
        [JsonProperty("rates")]
        public ICollection<NBPExchangeRate> Rates { get; set; }

        public static ICollection<NBPExchangeRateResponse> DeserializeNBPRates(string json)
        {
            return JsonConvert.DeserializeObject<ICollection<NBPExchangeRateResponse>>(json);
        }
    }
}
  