using IPM_239043.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace IPM_239043
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainWindowViewModel MainWindowViewModel { get; set; }
        public MainPage()
        {
            this.MainWindowViewModel = new MainWindowViewModel();
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["CurrentPageType"] = this.GetType().ToString();
            base.OnNavigatedTo(e);
        }

        private void NavigateToChartPage(object sender, RoutedEventArgs e)
        {
            List<string> parameters = new List<string>() { MainWindowViewModel.SelectedExchangeRate.Code };
            this.Frame.Navigate(typeof(ChartPage), parameters);
        }


        public void CloseApp(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }
    }
}
