using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IPM_239043.ViewModel
{
    public class SecondPageViewModel : INotifyPropertyChanged
    {
        private static SecondPageViewModel secondPageInstance;

        private string greeting;

        public string FName { get; set; }
        public string LName { get; set; }
        public string Greeting
        {
            get { return this.greeting; }
            set
            {
                this.greeting = value;
                Summary = "";
            }
        }
        public string Summary { get { return $"{this.Greeting}, {this.FName} {this.LName}"; }
            set
            {
                this.OnPropertyChanged();
            }
        }
        public SecondPageViewModel()
        {
            secondPageInstance = this;
        }

        public static SecondPageViewModel GetInstance()
        {
            return secondPageInstance;
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
