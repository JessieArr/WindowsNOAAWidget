using Newtonsoft.Json.Linq;

namespace WindowsNOAAWidget.Models.NOAA
{
    public class PointResponse
    {
        public JObject @Context { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public JObject Geometry { get; set; }
        public PointProperties Properties { get; set; }
    }
}
