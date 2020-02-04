using System;
using System.Collections.Generic;
using System.Text;
using WindowsNOAAWidget.Geolocation;
using Xunit;

namespace WindowsNOAAWidget.Test
{
    public class GeoDataImporterTests
    {
        [Fact]
        public void Test()
        {
            var x = GeoDataImporter.GetZipData();
            var simple = GeoDataImporter.SimplifyZipData(x);
            GeoDataImporter.WriteZipData(simple);
        }
    }
}
