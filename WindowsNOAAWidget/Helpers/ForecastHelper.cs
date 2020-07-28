using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Helpers
{
    public static class ForecastHelper
    {
        public static string GetFriendlyStringFromForecast(string metNoForecast)
        {
            var toLowerForecast = metNoForecast.ToLower();
            switch(toLowerForecast)
            {
                case "cloudy":
                    return "Cloudy";
                case "partlycloudy_night":
                    return "Partly Cloudy";
                case "rain":
                    return "Rain";
                case "lightrain":
                    return "Light Rain";
                case "heavyrain":
                    return "Heavy Rain";
                default:
                    return metNoForecast;
            }
        }
    }
}
