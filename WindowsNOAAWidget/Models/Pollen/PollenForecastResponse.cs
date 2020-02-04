using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Models.Pollen
{
    public class PollenForecastResponse
    {
        public string Type { get; set; }
        public DateTime ForecastDate { get; set; }
        public PollenForecastLocation Location { get; set; }
    }
}
