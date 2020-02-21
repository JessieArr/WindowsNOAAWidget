using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var zipFilePath = Path.Combine(applicationPath, _ZipCodeFileName);
                if (!File.Exists(zipFilePath))
                {
                    ErrorHelper.EmitError($"Unable to find {zipFilePath}");
                }
                var fileData = File.ReadAllText(zipFilePath);

                _ZipCodeInfo = JsonConvert.DeserializeObject<List<ZipCodeInfo>>(fileData);
            }
            return _ZipCodeInfo;
        }
    }
}
