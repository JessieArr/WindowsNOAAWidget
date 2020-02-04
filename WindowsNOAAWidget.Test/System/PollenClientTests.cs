using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Services;
using Xunit;

namespace WindowsNOAAWidget.Test.System
{
    public class PollenClientTests
    {
        [Fact]
        public async Task Test()
        {
            var sut = new PollenClient();
            var x = await sut.GetPollenDataForZip(12345);
        }
    }
}
