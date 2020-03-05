using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Models
{
    public class WeatherIconInfo
    {
        public string TemperatureInFarenheit { get; set; }
        public string ForecastDescription { get; set; }
        public bool IsDayTime { get; set; }
    }
}
