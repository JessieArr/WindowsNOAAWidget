using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
        private NOAAClient _NOAAClient;
        private PollenClient _PollenClient;
        private OptionsService _OptionsService;
        private GeographyService _GeographyService;
        private ApplicationOptions _ApplicationOptions;
        private string _MostRecentTemperature;

        public MainWindow()
        {
            InitializeComponent();
            _UpdateTimer = new Timer(60000);
            _UpdateTimer.Elapsed += _UpdateTimer_Elapsed;
            _UpdateTimer.AutoReset = true;
            _UpdateTimer.Enabled = true;

            _NOAAClient = new NOAAClient();
            _PollenClient = new PollenClient();
            _OptionsService = new OptionsService();
            _ApplicationOptions = _OptionsService.LoadSavedOptions();
            _GeographyService = new GeographyService();

            if(_ApplicationOptions.SelectedZip != null)
            {
                ZipTextBox.Text = _ApplicationOptions.SelectedZip.Zip.ToString();
            }

            ErrorHelper.ErrorLabel = ErrorText;
            ErrorHelper.EmitError("Errors will appear here.");

            SetIcon();
            UpdatePollenForecast();
        }

        private void _UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetIcon();
            UpdatePollenForecast();
        }

        private void UpdatePollenForecast()
        {
            Dispatcher.InvokeAsync(new Action(async () =>
            {
                if(_ApplicationOptions.SelectedZip != null)
                {
                    var pollenForecast = await _PollenClient.GetPollenDataForZip(_ApplicationOptions.SelectedZip.Zip);
                    if(!pollenForecast.Location.Periods.Any())
                    {
                        return;
                    }
                    PollenIndex.Content = "Pollen Index: " + pollenForecast.Location.Periods.First().Index;
                }
            }));
        }

        private void SetIcon()
        {
            Dispatcher.InvokeAsync(new Action(async () =>
            {
                int zip = 0;
                var wasInt = Int32.TryParse(ZipTextBox.Text, out zip);
                if(!wasInt)
                {
                    // Not a zip code
                    return;
                }

                if(_ApplicationOptions.SelectedZip == null || zip != _ApplicationOptions.SelectedZip.Zip)
                {
                    var allZipCodes = _GeographyService.GetZipCodeInfo();
                    var thisZipCode = allZipCodes.FirstOrDefault(x => x.Zip == zip);
                    if(thisZipCode == null)
                    {
                        // Invalid zip code
                        return;
                    }

                    _ApplicationOptions.SelectedZip = thisZipCode;
                    _OptionsService.SaveOptions(_ApplicationOptions);
                }
                try
                {
                    var pointInfo = await _NOAAClient.GetHourlyForecastForPoint(_ApplicationOptions.SelectedZip.Lat, _ApplicationOptions.SelectedZip.Lon);
                    if (pointInfo == null || pointInfo.Properties == null)
                    {
                        return;
                    }
                    var firstPeriod = pointInfo.Properties["periods"][0];

                    var temperature = firstPeriod["temperature"].ToString();
                    var forecastDescription = firstPeriod["shortForecast"].ToString();
                    var isDayTime = String.Equals(firstPeriod["isDaytime"].ToString(), "true", StringComparison.OrdinalIgnoreCase);
                    // If the last temperature was different from this one, we update our icon and window title.
                    if (string.IsNullOrEmpty(_MostRecentTemperature) || !String.Equals(_MostRecentTemperature, temperature))
                    {
                        _MostRecentTemperature = temperature;
                        var iconUri = new Uri("https://api.weather.gov/icons/land/day/few?size=medium", UriKind.RelativeOrAbsolute);

                        Bitmap bmp = null;
                        var lowercaseForecast = forecastDescription.ToLower();
                        if (lowercaseForecast.Contains("cloudy") || lowercaseForecast.Contains("fog"))
                        {
                            bmp = new Bitmap("./Images/cloudy.png");
                        }
                        if (bmp == null)
                        {
                            if (isDayTime)
                            {
                                bmp = new Bitmap("./Images/sunny.png");
                            }
                            else
                            {
                                bmp = new Bitmap("./Images/moon.png");
                            }
                        }

                        //var bmp = await _Client.GetImage(firstPeriod["icon"].ToString());

                        AddText(bmp, temperature);

                        var iconForBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            bmp.GetHbitmap(),
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

                        this.Icon = iconForBitmap;
                        this.Title = $"{temperature}° - {forecastDescription}";
                    }
                }
                catch(Exception ex)
                {
                    ErrorHelper.EmitError("ERROR: " + ex.Message);
                }                
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
