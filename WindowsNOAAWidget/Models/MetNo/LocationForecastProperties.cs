using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Models.MetNo
{
    public class LocationForecastProperties
    {
        public JObject meta { get; set; }
        public List<LocationForecastInstant> timeseries { get; set; }
    }
}
