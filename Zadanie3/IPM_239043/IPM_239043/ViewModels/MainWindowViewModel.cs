using NBPHttp;
using NBPHttp.ExchangeRateModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IPM_239043.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _selectedExchangeRateMid;
        private string _selectedExchangeRateConverter;
        private string _selectedExchangeDateText = "Data publikacji: ";
        private static readonly string API_URL = @"http://api.nbp.pl";
        private static readonly string EXCHANGE_RATES_EFFECTIVE_DATE_ENDPOINT = @"/api/exchangerates/rates/a/eur/2019-03-24/2019-04-24/";
        private static readonly string EXCHANGE_RATES_ENDPOINT = @"/api/exchangerates/tables/a/";
        private readonly NBPHttpClient _client;
        public MainWindowViewModel()
        {
            _client = new NBPHttpClient(new Uri(API_URL));
            ExchangeRates = new ObservableCollection<NBPExchangeRate>();
            EffectiveDates = new ObservableCollection<string>();
            GetListOfExchangeRatesForDate("");
        }

        public ObservableCollection<string> EffectiveDates { get; set; }
        public ObservableCollection<NBPExchangeRate> ExchangeRates { get; set; }
        public NBPExchangeRate SelectedExchangeRate { get; set; }
        public string SelectedEffectiveDateText
        {
            get
            {
                return _selectedExchangeDateText;
            }
            set
            {
                _selectedExchangeDateText = $"Data publikacji: {SelectedEffectiveDate}";
                OnPropertyChanged("SelectedEffectiveDateText");
            }
        }

        public string SelectedExchangeRateMid
        {
            get
            {
                return _selectedExchangeRateMid;
            }
            set
            {
                _selectedExchangeRateMid = value;
                OnPropertyChanged("SelectedExchangeRateMid");
            }
        }
        public string SelectedExchangeRateConverter
        {
            get
            {
                return _selectedExchangeRateConverter;
            }
            set
            {
                _selectedExchangeRateConverter = value;
                OnPropertyChanged("SelectedExchangeRateConverter");
            }
        }

        public void SetExchangeRateTextBoxes()
        {
            if (SelectedExchangeRate != null)
            {
                SelectedExchangeRateMid = SelectedExchangeRate.ConvertedMid;
                SelectedExchangeRateConverter = SelectedExchangeRate.Converter.ToString();
            }
        }

        public string SelectedEffectiveDate { get; set; }

        public async void GetListOfEffectiveDates()
        {
            var rates = await _client.GetNBPExchangeEffectiveDates(EXCHANGE_RATES_EFFECTIVE_DATE_ENDPOINT);
            EffectiveDates = new ObservableCollection<string>(rates.GetEffectiveDates());
            OnPropertyChanged("EffectiveDates");
        }

        public async void GetListOfExchangeRatesForDate(string date)
        {
            var rates = await _client.GetNBPExchangeRates(EXCHANGE_RATES_ENDPOINT, date);
            ExchangeRates.Clear();
            List<NBPExchangeRate> listOfRates = rates.FirstOrDefault()?.Rates.ToList();
            foreach (var rate in listOfRates)
                ExchangeRates.Add(rate);
            SelectedExchangeRate = ExchangeRates.FirstOrDefault();
        }

        public void GetRateForSelectedDate()
        {
            GetListOfExchangeRatesForDate(SelectedEffectiveDate);
            SelectedEffectiveDateText = SelectedEffectiveDate;
            OnPropertyChanged("ExchangeRates");
        }
    }
}
