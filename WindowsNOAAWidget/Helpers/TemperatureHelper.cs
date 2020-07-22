using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Helpers
{
    public static class TemperatureHelper
    {
        public static double ConvertCentrigradeToFarenheit(double temperatureinCentigrade)
        {
            return temperatureinCentigrade * 1.8 + 32;
        }
    }
}
