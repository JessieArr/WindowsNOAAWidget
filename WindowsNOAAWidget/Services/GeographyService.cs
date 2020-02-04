using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Geolocation;

namespace WindowsNOAAWidget.Services
{
    public class GeographyService
    {
        private static string _ZipCodeFileName = "Geolocation/zip-info.json";
        private static List<ZipCodeInfo> _ZipCodeInfo;

        public List<string> GetStates()
        {
            return new List<string>()
            {
                "FL"
            };
        }

        public List<ZipCodeInfo> GetZipCodeInfo()
        {
            if(_ZipCodeInfo == null)
            {
                var fileData = File.ReadAllText(_ZipCodeFileName);

                _ZipCodeInfo = JsonConvert.DeserializeObject<List<ZipCodeInfo>>(fileData);
            }
            return _ZipCodeInfo;
        }
    }
}
