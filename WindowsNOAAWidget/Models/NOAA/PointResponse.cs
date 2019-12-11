using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Models.NOAA
{
    public class PointResponse
    {
        public JObject @Context { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public JObject Geometry { get; set; }
        public JObject Properties { get; set; }
    }
}
