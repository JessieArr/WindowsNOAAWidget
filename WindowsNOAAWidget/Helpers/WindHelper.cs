using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Helpers
{
    public static class WindHelper
    {
        public static string GetWindDirectionFromDegrees(double degrees)
        {
            if(degrees < 0 || degrees > 360)
            {
                throw new Exception("How do circles work?");
            }
            if(degrees < 22 || degrees >= 337)
            {
                return "N";
            }
            if (degrees >= 22 && degrees < 67)
            {
                return "NE";
            }
            if (degrees >= 67 && degrees < 112)
            {
                return "E";
            }
            if (degrees >= 112 && degrees < 157)
            {
                return "SE";
            }
            if (degrees >= 157 && degrees < 202)
            {
                return "S";
            }
            if (degrees >= 202 && degrees < 247)
            {
                return "SW";
            }
            if (degrees >= 247 && degrees < 292)
            {
                return "W";
            }
            if (degrees >= 292 && degrees < 337)
            {
                return "NW";
            }
            return "?";
        }

        public static double ConvertMetersPerSecondToMPH(double metersPerSecond)
        {
            return metersPerSecond * 2.236936;
        }
    }
}
