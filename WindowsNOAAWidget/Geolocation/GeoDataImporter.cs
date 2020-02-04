using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Geolocation
{
    public static class GeoDataImporter
    {
        private static string _SourceFileName = "Geolocation/us-zip-code-latitude-and-longitude.json";
        private static string _OutputFileName = "Geolocation/zip-info.json";

        public static List<ZipCodeDetails> GetZipData()
        {
            var fileContents = File.ReadAllText(_SourceFileName);

            var deserializedContents = JsonConvert.DeserializeObject<List<ZipCodeDetails>>(fileContents);

            return deserializedContents;
        }

        public static List<ZipCodeInfo> SimplifyZipData(List<ZipCodeDetails> details)
        {
            var output = new List<ZipCodeInfo>();
            foreach(var detail in details)
            {
                output.Add(new ZipCodeInfo()
                {
                    City = (string)detail.fields["city"],
                    Zip = (int)detail.fields["zip"],
                    State = (string)detail.fields["state"],
                    Timezone = (int)detail.fields["timezone"],
                    Lat = (float)detail.fields["latitude"],
                    Lon = (float)detail.fields["longitude"],
                });
            }

            return output;
        }

        public static void WriteZipData(List<ZipCodeInfo> zipInfo)
        {
            var serializedContents = JsonConvert.SerializeObject(zipInfo);

            File.WriteAllText(_OutputFileName, serializedContents);
        }
    }
}
