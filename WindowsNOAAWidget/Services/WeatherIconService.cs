using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WindowsNOAAWidget.Models;

namespace WindowsNOAAWidget.Services
{
    public class WeatherIconService
    {
        public BitmapSource GetWeatherIcon(WeatherIconInfo info)
        {
            //var iconUri = new Uri("https://api.weather.gov/icons/land/day/few?size=medium", UriKind.RelativeOrAbsolute);

            Bitmap bmp = null;
            var lowercaseForecast = info.ForecastDescription.ToLower();
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (lowercaseForecast.Contains("cloudy") || lowercaseForecast.Contains("fog"))
            {
                bmp = new Bitmap(Path.Combine(applicationPath, "Images/cloudy.png"));
            }
            if (bmp == null)
            {
                if (info.IsDayTime)
                {
                    bmp = new Bitmap(Path.Combine(applicationPath, "Images/sunny.png"));
                }
                else
                {
                    bmp = new Bitmap(Path.Combine(applicationPath, "Images/moon.png"));
                }
            }

            //var bmp = await _Client.GetImage(firstPeriod["icon"].ToString());

            AddText(bmp, info.TemperatureInFarenheit);

            var iconForBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            return iconForBitmap;
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
            g.DrawString(text, new Font("Tahoma", fontSize), Brushes.Black, shadowRect);
            g.DrawString(text, new Font("Tahoma", fontSize), Brushes.Yellow, rectf);

            g.Flush();
        }
    }
}
