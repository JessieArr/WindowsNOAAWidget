using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Models.MetNo
{
    public class LocationForecastInstant
    {
        public DateTime time { get; set; }
        public LocationForecastInstantData data { get; set; }
    }
}
