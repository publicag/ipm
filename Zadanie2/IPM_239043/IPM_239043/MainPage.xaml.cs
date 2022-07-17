using IPM_239043.Logic;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IPM_239043
{
    public sealed partial class MainPage : Page
    {
        public Incrementor Incrementor { get; set; }

        CancellationTokenSource tokenSource;
        CancellationToken token;
        bool isRunning;
        public MainPage()
        {
            InitializeComponent();
            Incrementor = new Incrementor();
            textBox_Step.Text = Incrementor.Step;
            textBox_Hello.Text = Incrementor.Hello;
            button_Run.IsEnabled = !isRunning;
            button_Stop.IsEnabled = isRunning;
        }

        private void button_Run_Click(object sender, RoutedEventArgs e)
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            isRunning = true;
            button_Run.IsEnabled = !isRunning;
            button_Stop.IsEnabled = isRunning;

            Task.Factory.StartNew(async () => await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (Incrementor.Counter == 1000)
                    {
                        Incrementor.Counter = 0;
                    }
                    textBox_Step.Text = await Incrementor.IncrementCounter();

                }
                Incrementor.Counter = 0;
                textBox_Step.Text = Incrementor.Step;

            }), token);
        }

        private void button_Hello_Click(object sender, RoutedEventArgs e)
        {
            textBox_Hello.Text = Incrementor.IncrementHelloCounter();
        }

        private void button_Stop_Click(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
            isRunning = false;
            button_Run.IsEnabled = !isRunning;
            button_Stop.IsEnabled = isRunning;
        }
    }
}
