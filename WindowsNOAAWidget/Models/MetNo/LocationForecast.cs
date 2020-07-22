using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Models.MetNo
{
    public class LocationForecast
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public LocationForecastProperties properties { get; set; }
    }
}
