using System.Collections.Generic;

namespace WindowsNOAAWidget.Models.Pollen
{
    public class PollenForecastLocation
    {
        public string ZIP { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<PollenForecastPeriod> Periods { get; set; }
    }
}