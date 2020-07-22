using Newtonsoft.Json.Linq;

namespace WindowsNOAAWidget.Models.MetNo
{
    public class LocationForecastInstantData
    {
        public LocationForecastInstantDataDetailsContainer instant { get; set; }
        public JObject next_12_hours { get; set; }
        public JObject next_1_hours { get; set; }
        public JObject next_6_hours { get; set; }
    }
}