using NBPHttp;
using NBPHttp.ExchangeRateModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI.Popups;

namespace IPM_239043.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        ApplicationDataContainer localSettings;
        ApplicationDataCompositeValue composite;
        private DateTimeOffset _selectedDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 2);
        private string _selectedExchangeDateText = "Data publikacji: ";
        private static readonly string EXCHANGE_RATES_EFFECTIVE_DATE_ENDPOINT = @"/api/exchangerates/rates/a/eur/";
        private static readonly string EXCHANGE_RATES_ENDPOINT = @"/api/exchangerates/tables/a/";
        private readonly NBPHttpClient _client;
        private static MainWindowViewModel _instance;
        private string _selectedEffectiveDate;

        public MainWindowViewModel()
        {
            _client = NBPHttpClient.GetInstance();
            ExchangeRates = new ObservableCollection<NBPExchangeRate>();
            EffectiveDates = new ObservableCollection<string>();
            localSettings = ApplicationData.Current.LocalSettings;
            composite = (ApplicationDataCompositeValue)localSettings.Values["MainWindowViewModel"];
            if (composite == null)
            {
                composite = new ApplicationDataCompositeValue();
                this.StoreLocalSettings();
            }
            else
            {
                _selectedEffectiveDate = (String)composite["_selectedEffectiveDate"];
            }
            GetListOfExchangeRatesForDate(_selectedEffectiveDate);
        }

        public static MainWindowViewModel GetInstance()
        {
            return _instance;
        }

        public ObservableCollection<string> EffectiveDates { get; set; }
        public ObservableCollection<NBPExchangeRate> ExchangeRates { get; set; }
        public NBPExchangeRate SelectedExchangeRate { get; set; }

        public DateTimeOffset MinDate
        {
            get
            {
                return new DateTime(2005, 1, 2);
            }
        }

        public DateTimeOffset MaxDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        public DateTimeOffset SelectedDateTime
        {
            get
            {
                return _selectedDateTime;
            }
            set
            {
                _selectedDateTime = value;
                OnPropertyChanged("SelectedDateTime");
            }
        }
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

        public string SelectedEffectiveDate
        {
            get
            {
                return _selectedEffectiveDate;
            }
            set
            {
                _selectedEffectiveDate = value;
                StoreLocalSettings();
            }
        }

        public async void GetListOfEffectiveDates()
        {
            var dates = CreateDateOffsetMonth();
            var request = EXCHANGE_RATES_EFFECTIVE_DATE_ENDPOINT + dates;
            NBPExchangeEffectiveDates rates = null;
            try
            {
                rates = await _client.GetNBPExchangeEffectiveDates(request);
            }
            catch (NBPExchangeRatesException)
            {
                var messageDialog = new MessageDialog($"Brak danych dla podanego miesiąca.");
                await messageDialog.ShowAsync();
            }
            if (rates != null)
            {
                EffectiveDates = new ObservableCollection<string>(rates.GetEffectiveDates());
                OnPropertyChanged("EffectiveDates");
            }
        }

        public async void GetListOfExchangeRatesForDate(string date)
        {
            var rates = await _client.GetNBPExchangeRates(EXCHANGE_RATES_ENDPOINT, date);
            ExchangeRates.Clear();
            List<NBPExchangeRate> listOfRates = rates.FirstOrDefault()?.Rates.ToList();
            foreach (var rate in listOfRates)
                ExchangeRates.Add(rate);
        }

        public void GetRateForSelectedDate()
        {
            GetListOfExchangeRatesForDate(SelectedEffectiveDate);
            SelectedEffectiveDateText = SelectedEffectiveDate;
            OnPropertyChanged("ExchangeRates");
        }

        public void StoreLocalSettings()
        {
            composite["_selectedEffectiveDate"] = _selectedEffectiveDate;
            localSettings.Values["MainWindowViewModel"] = composite;
        }

        private string CreateDateOffsetMonth()
        {
            return _selectedDateTime.Date.Month == DateTime.Now.Date.Month ? _selectedDateTime.ToString("yyyy-MM-dd") + @"/"
                 + _selectedDateTime.AddDays(DateTime.Now.Day - 2).ToString("yyyy-MM-dd") + @"/"
                 : _selectedDateTime.ToString("yyyy-MM-dd") + @"/" +
                 _selectedDateTime.AddDays(DateTime.DaysInMonth(_selectedDateTime.Year, _selectedDateTime.Month)).ToString("yyyy-MM-dd") + @"/";
        }
    }
}
