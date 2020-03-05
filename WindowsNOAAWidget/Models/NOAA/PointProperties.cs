using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Models.NOAA
{
    public class PointProperties
    {
        public DateTime updated { get; set; }
        public string units { get; set; }
        public string forecastGenerator { get; set; }
        public DateTime generatedAt { get; set; }
        public DateTime updateTime { get; set; }
        public JValue validTimes { get; set; }
        public JObject elevation { get; set; }
        public List<ForecastPeriod> periods { get; set; }
    }
}
