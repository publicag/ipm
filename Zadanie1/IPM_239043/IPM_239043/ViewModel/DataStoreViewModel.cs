using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace IPM_239043.ViewModel
{
    public class DataStoreViewModel : INotifyPropertyChanged
    {
        private string fname;
        private string lname;
        private string lifeHistory;
        ApplicationDataContainer localSettings;
        ApplicationDataCompositeValue composite;
        private static DataStoreViewModel myInstance;
        public readonly string _clearLifeHistoryBtn = "Clear life history";
        public readonly string _lifeCycleHistoryDesc = "1-launched; 2-restored; 3-suspended; 4-resumed";
        public static DataStoreViewModel GetInstance()
        {
            return myInstance;
        }

        public DataStoreViewModel()
        {
            myInstance = this;
            fname = "UWP";
            lname = "User";
            localSettings = ApplicationData.Current.LocalSettings;
            composite = (ApplicationDataCompositeValue)localSettings.Values["DataBindingViewModel"];
            if (composite == null)
            {
                composite = new ApplicationDataCompositeValue();
                this.StoreLocalSettings();
            }
            else
            {
                fname = (String)composite["fname"];
                lname = (String)composite["lname"];
                lifeHistory = (String)composite["lifeHistory"];
            }
        }

        public string FName
        {
            get { return this.fname; }
            set
            {
                this.fname = value;
                Summary = "";
                this.StoreLocalSettings();
            }
        }

        public string LName
        {
            get { return this.lname; }
            set
            {
                this.lname = value;
                Summary = "";
                this.StoreLocalSettings();
            }
        }

        public string LifeHistory
        {
            get { return "LifeHistory: " + this.lifeHistory; }
            set
            {
                if (value.CompareTo("launched") == 0)
                    this.lifeHistory += "1 ";
                else if (value.CompareTo("restored") == 0)
                    this.lifeHistory += "2 ";
                else if (value.CompareTo("suspended") == 0)
                    this.lifeHistory += "3 ";
                else if (value.CompareTo("resumed") == 0)
                    this.lifeHistory += "4 ";
                StoreLocalSettings();
                this.OnPropertyChanged();
            }
        }

        public string Greeting { get; set; } = "Witaj";

        public void StoreLocalSettings()
        {
            composite["fname"] = fname;
            composite["lname"] = lname;
            composite["lifeHistory"] = lifeHistory;
            localSettings.Values["DataBindingViewModel"] = composite;
        }

        public string Summary
        {
            get { return $"{this.Greeting}, {this.FName} {this.LName}"; }
            set 
            { 
                this.OnPropertyChanged();
            }
        }
        public void ClearLifeHistory()
        {
            composite["lifeHistory"] = "";
            this.lifeHistory = "";
            OnPropertyChanged("LifeHistory");
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
