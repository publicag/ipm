using NBPHttp;
using NBPHttp.ExchangeRateModel;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Popups;

namespace IPM_239043.ViewModels
{
    public class ChartWindowViewModel : BaseViewModel
    {
        ApplicationDataContainer localSettings;
        ApplicationDataCompositeValue composite;
        private string _currencyTag;
        private DateTimeOffset _selectedFromDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        private DateTimeOffset _selectedToDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private static readonly string EXCHANGE_RATES_ENDPOINT = @"/api/exchangerates/rates/a/";
        private readonly NBPHttpClient _client;
        private List<ChartModel> _chartModel;
        private static ChartWindowViewModel instance;

        public string CurrencyHistory
        {
            get
            {
                return $"Historia waluty {_currencyTag}/PLN";
            }
            set
            {
                OnPropertyChanged();
            }
        }

        public static ChartWindowViewModel GetInstance()
        {
            return instance;
        }

        public ChartWindowViewModel()
        {
            _client = NBPHttpClient.GetInstance();
            instance = this;
            localSettings = ApplicationData.Current.LocalSettings;
            composite = (ApplicationDataCompositeValue)localSettings.Values["ChartWindowViewModel"];
            if (composite == null)
            {
                composite = new ApplicationDataCompositeValue();
                this.StoreLocalSettings();
            }
            else
            {
                _currencyTag = (String)composite["_currencyTag"];
                _selectedFromDateTime = (DateTimeOffset)composite["_selectedFromDateTime"];
                _selectedToDateTime = (DateTimeOffset)composite["_selectedToDateTime"];
            }
        }

        public int MaxProgresBar { get; set; } = 1;
        public int ValueProgresBar { get; set; } = 0;

        public List<ChartModel> ChartModel
        {
            get
            {
                return _chartModel;
            }
            set
            {
                _chartModel = value;
                OnPropertyChanged("ChartModel");
            }
        }
        public NBPExchangeRateHistory ExchangeRateHistory { get; set; }
        public string CurrencyTag
        {
            get
            {
                return _currencyTag;
            }
            set
            {
                _currencyTag = value;
                StoreLocalSettings();
                OnPropertyChanged("CurrencyTag");
            }
        }
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

        public DateTimeOffset SelectedFromDateTime
        {
            get
            {
                return _selectedFromDateTime;
            }
            set
            {
                _selectedFromDateTime = value;
                StoreLocalSettings();
                OnPropertyChanged("SelectedFromDateTime");
            }
        }

        public DateTimeOffset SelectedToDateTime
        {
            get
            {
                return _selectedToDateTime;
            }
            set
            {
                _selectedToDateTime = value;
                StoreLocalSettings();
                OnPropertyChanged("SelectedToDateTime");
            }
        }

        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }

        public async void GetExchangeRateHistory()
        {
            NBPExchangeRateHistory rates = null;
            var fromDate = _selectedFromDateTime.ToString("yyyy-MM-dd");
            var toDate = _selectedToDateTime.ToString("yyyy-MM-dd");
            DateTimeOffset selectedDateTimeOffset = _selectedFromDateTime;
            try
            {
                if (_selectedFromDateTime >= _selectedToDateTime)
                    throw new NBPExchangeRatesException();

                if ((_selectedToDateTime - _selectedFromDateTime).TotalDays > 365)
                {
                    NBPExchangeRateHistory fromToRates = null;

                    while (selectedDateTimeOffset < _selectedToDateTime)
                    {
                        var selectedFrom = selectedDateTimeOffset.ToString("yyyy-MM-dd");
                        var selectedFromYear = selectedDateTimeOffset.AddDays(365).ToString("yyyy-MM-dd");
                        if (selectedDateTimeOffset.AddDays(365) > DateTime.Now)
                        {
                            selectedFromYear = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        if (rates == null)
                            rates = await _client.GetNBPExchangeHistoryRate(EXCHANGE_RATES_ENDPOINT, _currencyTag, selectedFrom, selectedFromYear);
                        else
                        {
                            fromToRates = await _client.GetNBPExchangeHistoryRate(EXCHANGE_RATES_ENDPOINT, _currencyTag, selectedFrom, selectedFromYear);
                            foreach(var rate in fromToRates.Rates)
                            {
                                rates.Rates.Add(rate);
                            }
                        }
                        selectedDateTimeOffset = selectedDateTimeOffset.AddDays(365);
                    }
                } 
                else
                {
                    rates = await _client.GetNBPExchangeHistoryRate(EXCHANGE_RATES_ENDPOINT, _currencyTag, fromDate, toDate);
                }
            }
            catch (NBPExchangeRatesException)
            {
                var messageDialog = new MessageDialog("Brak danych dla podanego przedziału.");
                await messageDialog.ShowAsync();
            }
            if (rates != null)
            {
                ExchangeRateHistory = rates;
                LoadChartContents();
            }
        }

        public void StoreLocalSettings()
        {
            composite["_currencyTag"] = _currencyTag;
            composite["_selectedFromDateTime"] = _selectedFromDateTime;
            composite["_selectedToDateTime"] = _selectedToDateTime;
            localSettings.Values["ChartWindowViewModel"] = composite;
        }

        private void LoadChartContents()
        {
            List<ChartModel> source = new List<ChartModel>();
            foreach (var rate in ExchangeRateHistory.Rates)
            {
                source.Add(new ChartModel(rate.EffectiveDate, Convert.ToDouble(rate.Mid)));
            }

            ChartModel = source;
        }
    }
}
