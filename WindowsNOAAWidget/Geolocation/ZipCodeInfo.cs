using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Geolocation
{
    public class ZipCodeInfo
    {
        public int Zip { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Timezone { get; set; }
    }
}
