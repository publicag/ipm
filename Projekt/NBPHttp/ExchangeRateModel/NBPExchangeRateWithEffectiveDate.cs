using Newtonsoft.Json;

namespace NBPHttp.ExchangeRateModel
{
    public class NBPExchangeRateWithEffectiveDate
    {
        [JsonProperty("no")]
        public string No { get; set; }
        [JsonProperty("effectiveDate")]
        public string EffectiveDate { get; set; }
        [JsonProperty("mid")]
        public decimal Mid { get; set; }
    }
}
