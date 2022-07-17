using IPM_239043.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace IPM_239043
{
    public sealed partial class ChartPage : Page
    {
        ChartWindowViewModel ChartWindowModel { get; set; }
        private bool _isSwiped;

        public ChartPage()
        {
            this.ChartWindowModel = new ChartWindowViewModel();
            this.InitializeComponent();
        }

        public void CloseApp(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["CurrentPageType"] = this.GetType().ToString();
            if (e.Parameter is List<string>)
            {
                var parameters = e.Parameter as List<string>;
                ChartWindowModel.CurrencyTag = parameters[0];
            }
            base.OnNavigatedTo(e);
        }

        private void NavigateToMainPage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(ChartPageMain);
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

            var savePicker = new FileSavePicker();
            savePicker.DefaultFileExtension = ".jpg";
            savePicker.FileTypeChoices.Add(".jpg", new List<string> { ".jpg" });
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.SuggestedFileName = "wykres.jpg";

            var saveFile = await savePicker.PickSaveFileAsync();

            if (saveFile == null)
                return;

            using (var fileStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);

                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    pixelBuffer.ToArray());

                await encoder.FlushAsync();
            }
        }

        private void SwipeRight(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e.IsInertial && !_isSwiped)
            {
                var swipedDistance = e.Cumulative.Translation.X;

                if (swipedDistance > 0 && Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }

                _isSwiped = true;
            }
        }

        private void SwipeCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _isSwiped = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChartWindowModel.GetExchangeRateHistory();
        }
    }
}
