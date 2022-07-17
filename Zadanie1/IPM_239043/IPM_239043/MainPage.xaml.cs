using IPM_239043.ViewModel;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IPM_239043
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = DataStoreViewModel.GetInstance();
        }

        public DataStoreViewModel ViewModel { get; set; }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> parameters = new List<string>() { ViewModel.Greeting, ViewModel.FName, ViewModel.LName };
            this.Frame.Navigate(typeof(SecondPage), parameters);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrEmpty(e.Parameter as string))
            {
                ViewModel.Greeting = e.Parameter as string;
            }
            base.OnNavigatedTo(e);
        }
    }

}