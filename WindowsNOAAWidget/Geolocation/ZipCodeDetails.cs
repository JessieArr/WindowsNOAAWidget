using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Geolocation
{
    public class ZipCodeDetails
    {
        public string datasetid { get; set; }
        public string recordid { get; set; }
        public JObject fields { get; set; }
        public JObject geometry { get; set; }
        public DateTime record_timestamp { get; set; }
    }
}
