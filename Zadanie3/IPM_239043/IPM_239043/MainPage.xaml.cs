using IPM_239043.ViewModels;
using Windows.UI.Xaml.Controls;

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
    }
}
