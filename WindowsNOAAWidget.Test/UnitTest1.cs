using System;
using System.Threading.Tasks;
using WindowsNOAAWidget.Services;
using Xunit;

namespace WindowsNOAAWidget.Test
{
    public class UnitTest1
    {
        public NOAAClient _SUT;

        public UnitTest1()
        {
            _SUT = new NOAAClient();
        }

        [Fact]
        public async Task Test1()
        {
            var result = await _SUT.GetForecastForPoint(39.7456, -97.0892);
        }
    }
}
