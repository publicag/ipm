using IPM_239043.ViewModel;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace IPM_239043
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class SecondPage : Page
    {
        public SecondPage()
        {
            this.InitializeComponent();
            this.ViewModel = SecondPageViewModel.GetInstance(); 
        }

        public SecondPageViewModel ViewModel { get; set; }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), ViewModel.Greeting);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is List<string>)
            {
                var parameters = e.Parameter as List<string>;
                ViewModel.Greeting = parameters[0];
                ViewModel.FName = parameters[1];
                ViewModel.LName = parameters[2];
            }
            BackButton.IsEnabled = this.Frame.CanGoBack;
            base.OnNavigatedTo(e);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                HyperlinkButton_Click(sender, e);
            }
        }

    }
}
