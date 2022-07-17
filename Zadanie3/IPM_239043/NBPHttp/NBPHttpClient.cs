using NBPHttp.ExchangeRateModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NBPHttp
{
    public class NBPHttpClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public NBPHttpClient(Uri uri)
        {
            _httpClient.BaseAddress = uri;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ICollection<NBPExchangeRateResponse>> GetNBPExchangeRates(string endpoint, string date)
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync($"{endpoint}{date}").Result;
                response.EnsureSuccessStatusCode();
                var rates = await response.Content.ReadAsStringAsync();
                return NBPExchangeRateResponse.DeserializeNBPRates(rates);
            }
            catch (HttpRequestException e)
            {
                throw new NBPExchangeRatesException(e.Message);
            }
        }

        public async Task<NBPExchangeEffectiveDates> GetNBPExchangeEffectiveDates(string endpoint)
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(endpoint).Result;
                response.EnsureSuccessStatusCode();
                var effectiveDates = await response.Content.ReadAsStringAsync();
                return NBPExchangeEffectiveDates.DeserializeNBPEffectiveDates(effectiveDates);
            }
            catch (HttpRequestException e)
            {
                throw new NBPExchangeRatesException(e.Message);
            }
        }
    }
}
