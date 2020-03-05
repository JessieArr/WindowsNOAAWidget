using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Services;
using Xunit;

namespace WindowsNOAAWidget.Test.System
{
    public class NOAAClientTests
    {
        public NOAAClient _SUT;

        public NOAAClientTests()
        {
            _SUT = new NOAAClient();
        }

        [Fact]
        public async Task GetForecastForPoint_DoesNotThrow()
        {
            var result = await _SUT.GetForecastForPoint(39.7456, -97.0892);
        }

        [Fact]
        public async Task GetHourlyForecastForPoint_DoesNotThrow()
        {
            var result = await _SUT.GetHourlyForecastForPoint(39.7456, -97.0892);
        }
    }
}
