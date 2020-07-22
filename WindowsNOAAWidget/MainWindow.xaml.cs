using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WindowsNOAAWidget.Helpers;
using WindowsNOAAWidget.Models;
using WindowsNOAAWidget.Models.MetNo;
using WindowsNOAAWidget.Models.NOAA;
using WindowsNOAAWidget.Models.Pollen;
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
        private MetNoClient _MetNoClient;
        private PollenClient _PollenClient;
        private OptionsService _OptionsService;
        private GeographyService _GeographyService;
        private ApplicationOptions _ApplicationOptions;
        private WeatherIconService _WeatherIconService;
        private string _MostRecentTemperature;
        private bool UseMetNoAPI = true;

        private PollenForecastResponse _PollenForecast;
        private TimeSpan _PollenForecastLifetime = new TimeSpan(1, 0, 0);
        private DateTime _PollenForecastExpires;

        public MainWindow()
        {
            InitializeComponent();
            _UpdateTimer = new Timer(60000);
            _UpdateTimer.Elapsed += _UpdateTimer_Elapsed;
            _UpdateTimer.AutoReset = true;
            _UpdateTimer.Enabled = true;

            _NOAAClient = new NOAAClient();
            _MetNoClient = new MetNoClient();
            _PollenClient = new PollenClient();
            _OptionsService = new OptionsService();
            _ApplicationOptions = _OptionsService.LoadSavedOptions();
            _GeographyService = new GeographyService();
            _WeatherIconService = new WeatherIconService();

            if (_ApplicationOptions.SelectedZip != null)
            {
                ZipTextBox.Text = _ApplicationOptions.SelectedZip.Zip.ToString();
            }

            ErrorHelper.ErrorLabel = ErrorText;
            ErrorHelper.EmitError("Errors will appear here.");

            GetWeatherForecast();
            UpdatePollenForecast();
        }

        private void _UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetWeatherForecast();
            UpdatePollenForecast();
        }

        private void UpdatePollenForecast()
        {
            Dispatcher.InvokeAsync(new Action(async () =>
            {
                try
                {
                    if (_ApplicationOptions.SelectedZip != null && DateTime.Now > _PollenForecastExpires)
                    {
                        var pollenForecast = await _PollenClient.GetPollenDataForZip(_ApplicationOptions.SelectedZip.Zip);
                        if (pollenForecast?.Location?.Periods == null || !pollenForecast.Location.Periods.Any())
                        {
                            return;
                        }
                        PollenIndex.Content = "Pollen Index: " + pollenForecast.Location.Periods.First().Index;
                        _PollenForecast = pollenForecast;
                        _PollenForecastExpires = DateTime.Now + _PollenForecastLifetime;
                    }
                }
                catch (Exception ex)
                {
                    ErrorHelper.EmitError(ex);
                }
            }));
        }

        private void GetWeatherForecast()
        {
            Dispatcher.InvokeAsync(new Action(async () =>
            {
                try
                {
                    int zip = 0;
                    var wasInt = Int32.TryParse(ZipTextBox.Text, out zip);
                    if (!wasInt)
                    {
                        // Not a zip code
                        return;
                    }

                    if (_ApplicationOptions.SelectedZip == null || zip != _ApplicationOptions.SelectedZip.Zip)
                    {
                        var allZipCodes = _GeographyService.GetZipCodeInfo();
                        var thisZipCode = allZipCodes.FirstOrDefault(x => x.Zip == zip);
                        if (thisZipCode == null)
                        {
                            // Invalid zip code
                            return;
                        }

                        _ApplicationOptions.SelectedZip = thisZipCode;
                        _OptionsService.SaveOptions(_ApplicationOptions);
                    }

                    if(UseMetNoAPI)
                    {
                        var forecast = await _MetNoClient.GetWeatherInfoForPoint(_ApplicationOptions.SelectedZip.Lat, _ApplicationOptions.SelectedZip.Lon);
                        var next24Hours = forecast.properties.timeseries.Take(24);

                        HourlyForecastStack.Children.Clear();
                        HourlyForecastStack.Children.Add(GetGridHeader());
                        foreach (var hour in next24Hours)
                        {
                            HourlyForecastStack.Children.Add(GetGridForLocationForecastInstant(hour));
                        }

                        var firstPeriod = forecast.properties.timeseries.First();

                        var temperature = firstPeriod.data.instant.details.air_temperature;
                        var temperatureFarenheit = TemperatureHelper.ConvertCentrigradeToFarenheit(temperature);
                        var temperatureString = Math.Round(temperatureFarenheit).ToString();
                        var forecastDescription = firstPeriod.data.next_1_hours["summary"]["symbol_code"].ToString();
                        var isDayTime = DateTime.Now.TimeOfDay.Hours > 7 && DateTime.Now.TimeOfDay.Hours < 19;
                        // If the last temperature was different from this one, we update our icon and window title.
                        if (string.IsNullOrEmpty(_MostRecentTemperature) || !String.Equals(_MostRecentTemperature, temperature))
                        {
                            _MostRecentTemperature = temperatureString;

                            var icon = _WeatherIconService.GetWeatherIcon(new WeatherIconInfo()
                            {
                                TemperatureInFarenheit = temperatureString,
                                ForecastDescription = forecastDescription,
                                IsDayTime = isDayTime
                            });

                            this.Icon = icon;
                            this.Title = $"{temperatureString}° - {forecastDescription}";
                        }
                    }
                    else
                    {
                        var pointInfo = await _NOAAClient.GetHourlyForecastForPoint(_ApplicationOptions.SelectedZip.Lat, _ApplicationOptions.SelectedZip.Lon);
                        if (pointInfo == null || pointInfo.Properties == null)
                        {
                            return;
                        }

                        // Populate forecast
                        var next24Hours = pointInfo.Properties.periods.Take(24);
                        HourlyForecastStack.Children.Clear();
                        HourlyForecastStack.Children.Add(GetGridHeader());
                        foreach (var hour in next24Hours)
                        {
                            HourlyForecastStack.Children.Add(GetGridForForecastPeriod(hour));
                        }

                        var firstPeriod = pointInfo.Properties.periods[0];

                        var temperature = firstPeriod.temperature.ToString();
                        var forecastDescription = firstPeriod.shortForecast;
                        var isDayTime = DateTime.Now.TimeOfDay.Hours > 7 && DateTime.Now.TimeOfDay.Hours < 19;
                        // If the last temperature was different from this one, we update our icon and window title.
                        if (string.IsNullOrEmpty(_MostRecentTemperature) || !String.Equals(_MostRecentTemperature, temperature))
                        {
                            _MostRecentTemperature = temperature;

                            var icon = _WeatherIconService.GetWeatherIcon(new WeatherIconInfo()
                            {
                                TemperatureInFarenheit = temperature,
                                ForecastDescription = forecastDescription,
                                IsDayTime = isDayTime
                            });

                            this.Icon = icon;
                            this.Title = $"{temperature}° - {forecastDescription}";
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    ErrorHelper.EmitError(ex);
                }
            }));
        }

        private Grid GetGridHeader()
        {
            var newGrid = new Grid();
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(80)
            });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(50)
            });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(80)
            });

            var timeLabel = new Label();
            timeLabel.Content = "Time";
            Grid.SetColumn(timeLabel, 0);
            newGrid.Children.Add(timeLabel);

            var temperatureLabel = new Label();
            temperatureLabel.Content = "Temp";
            Grid.SetColumn(temperatureLabel, 1);
            newGrid.Children.Add(temperatureLabel);

            var descriptionLabel = new Label();
            descriptionLabel.Content = "Forecast";
            Grid.SetColumn(descriptionLabel, 2);
            newGrid.Children.Add(descriptionLabel);

            var windLabel = new Label();
            windLabel.Content = "Wind";
            Grid.SetColumn(windLabel, 3);
            newGrid.Children.Add(windLabel);

            return newGrid;
        }

        private Grid GetGridForForecastPeriod(ForecastPeriod period)
        {
            var newGrid = new Grid();
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(80)
            });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(50)
            });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(80)
            });

            var timeLabel = new Label();
            timeLabel.Content = period.startTime.ToString("MM/dd htt");
            Grid.SetColumn(timeLabel, 0);
            newGrid.Children.Add(timeLabel);

            var temperatureLabel = new Label();
            temperatureLabel.Content = period.temperature + "°" + period.temperatureUnit;
            Grid.SetColumn(temperatureLabel, 1);
            newGrid.Children.Add(temperatureLabel);

            var descriptionLabel = new Label();
            descriptionLabel.Content = period.shortForecast;
            Grid.SetColumn(descriptionLabel, 2);
            newGrid.Children.Add(descriptionLabel);

            var windLabel = new Label();
            windLabel.Content = $"{period.windDirection} {period.windSpeed}";
            Grid.SetColumn(windLabel, 3);
            newGrid.Children.Add(windLabel);

            return newGrid;
        }

        private Grid GetGridForLocationForecastInstant(LocationForecastInstant instant)
        {
            var instantDetails = instant.data.instant.details;
            var newGrid = new Grid();
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(80)
            });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(50)
            });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition());
            newGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(80)
            });

            var timeLabel = new Label();
            timeLabel.Content = instant.time.ToLocalTime().ToString("MM/dd htt");
            Grid.SetColumn(timeLabel, 0);
            newGrid.Children.Add(timeLabel);

            var temperatureLabel = new Label();
            var farenheitTemperature = TemperatureHelper.ConvertCentrigradeToFarenheit(instantDetails.air_temperature);
            temperatureLabel.Content = Math.Round(farenheitTemperature, 1) + "°F";
            Grid.SetColumn(temperatureLabel, 1);
            newGrid.Children.Add(temperatureLabel);

            var descriptionLabel = new Label();
            descriptionLabel.Content = instant.data.next_1_hours["summary"]["symbol_code"];
            Grid.SetColumn(descriptionLabel, 2);
            newGrid.Children.Add(descriptionLabel);

            var windLabel = new Label();
            var windCardinality = WindHelper.GetWindDirectionFromDegrees(instantDetails.wind_from_direction);
            var windSpeedInMPH = Math.Round(WindHelper.ConvertMetersPerSecondToMPH(instantDetails.wind_speed), 1);
            windLabel.Content = $"{windCardinality} {windSpeedInMPH} MPH";
            Grid.SetColumn(windLabel, 3);
            newGrid.Children.Add(windLabel);

            return newGrid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetWeatherForecast();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        private void Startup_Click(object sender, RoutedEventArgs e)
        {
            var startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var bootstrapperPath = Path.Combine(applicationPath, "WindowsNOAABootstrapper.exe");
            WindowsShortcutService.CreateShortcut("WindowsNOAAWidget", startupPath, bootstrapperPath);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
