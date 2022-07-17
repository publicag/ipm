using Newtonsoft.Json;

namespace NBPHttp.ExchangeRateModel
{
    public class NBPExchangeRate
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("mid")]
        public decimal Mid { get; set; }
        public int Converter
        {
            get
            {
                if (Mid < 0.10m)
                    return 100;
                else
                    return 1;
            }
            set { }
        }

        public string ConvertedMid
        {
            get
            {
                return (Mid * Converter).ToString();
            }
            set { }
        }

        public string Title
        {
            get
            {
                return $"{Code} - {Currency}";
            }
            set { }
        }

        public string SubTitle
        {
            get
            {
                return $"{ConvertedMid} - Przelicznik: {Converter}";
            }
            set { }
        }
    }
}
