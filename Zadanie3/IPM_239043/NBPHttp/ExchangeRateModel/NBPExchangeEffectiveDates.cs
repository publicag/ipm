using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace NBPHttp.ExchangeRateModel
{
    public class NBPExchangeEffectiveDates
    {
        [JsonProperty("rates")]
        public ICollection<NBPExchangeRateWithEffectiveDate> EffectiveDates { get; set; }

        public static NBPExchangeEffectiveDates DeserializeNBPEffectiveDates(string json)
        {
            return JsonConvert.DeserializeObject<NBPExchangeEffectiveDates>(json);
        }

        public ICollection<string> GetEffectiveDates()
        {
            return EffectiveDates.Select(x => x.EffectiveDate).ToList();
        }

    }
}
