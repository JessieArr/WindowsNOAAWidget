using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Services;
using Xunit;

namespace WindowsNOAAWidget.Test.SystemTests
{
    public class MetNoClientTests
    {
        public MetNoClient _SUT;

        public MetNoClientTests()
        {
            _SUT = new MetNoClient();
        }

        [Fact]
        public async Task GetForecastForPoint_DoesNotThrow()
        {
            var result = await _SUT.GetWeatherInfoForPoint(39.7456, -97.0892);
        }
    }
}
