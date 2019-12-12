using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;
using WindowsNOAAWidget.Models;
using WindowsNOAAWidget.Services;

namespace WindowsNOAAWidget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _UpdateTimer;
        private NOAAClient _Client;
        private OptionsService _OptionsService;
        private ApplicationOptions _ApplicationOptions;

        public MainWindow()
        {
            InitializeComponent();
            _UpdateTimer = new Timer(60000);
            _UpdateTimer.Elapsed += _UpdateTimer_Elapsed;
            _UpdateTimer.AutoReset = true;
            _UpdateTimer.Enabled = true;

            _Client = new NOAAClient();
            _OptionsService = new OptionsService();
            _ApplicationOptions = _OptionsService.LoadSavedOptions();

            LatitudeTextBox.Text = _ApplicationOptions.SelectedLatitude.ToString();
            LongitudeTextBox.Text = _ApplicationOptions.SelectedLongitude.ToString();

            ErrorHelper.ErrorLabel = ErrorText;
            ErrorHelper.EmitError("Errors will appear here.");

            SetIcon();
        }

        private void _UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetIcon();
        }

        private void SetIcon()
        {
            Dispatcher.InvokeAsync(new Action(async () =>
            {
                double latitude = 0;
                double longitude = 0;
                Double.TryParse(LatitudeTextBox.Text, out latitude);
                Double.TryParse(LongitudeTextBox.Text, out longitude);

                if(latitude != _ApplicationOptions.SelectedLatitude 
                    || longitude != _ApplicationOptions.SelectedLongitude)
                {
                    _ApplicationOptions.SelectedLatitude = latitude;
                    _ApplicationOptions.SelectedLongitude = longitude;
                    _OptionsService.SaveOptions(_ApplicationOptions);
                }

                if(latitude == 0 && longitude == 0)
                {
                    // No point has been entered, so we wait.
                    return;
                }
                var pointInfo = await _Client.GetHourlyForecastForPoint(latitude, longitude);
                var firstPeriod = pointInfo.Properties["periods"][0];

                var temperature = firstPeriod["temperature"].ToString();

                //var iconUri = new Uri("pack://application:,,,/Images/sunny.png", UriKind.RelativeOrAbsolute);
                var iconUri = new Uri("https://api.weather.gov/icons/land/day/few?size=medium", UriKind.RelativeOrAbsolute);
                var bmp = new Bitmap("./Images/sunny.png");

                //var bmp = await _Client.GetImage(firstPeriod["icon"].ToString());

                AddText(bmp, temperature);

                var iconForBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                this.Icon = iconForBitmap;
                this.Title = $"{temperature}° - {firstPeriod["shortForecast"]}";
            }));
        }

        private void AddText(Bitmap bmp, string text)
        {
            var fontSize = 45;
            var rectf = new RectangleF(0, 0, 128, 128);
            var shadowRect = new RectangleF(5, 5, 128, 128);

            var g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(text, new Font("Tahoma", fontSize), System.Drawing.Brushes.Black, shadowRect);
            g.DrawString(text, new Font("Tahoma", fontSize), System.Drawing.Brushes.Yellow, rectf);

            g.Flush();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetIcon();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
