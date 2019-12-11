using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAAWidget.Services
{
    public class GeographyService
    {
        public List<string> GetStates()
        {
            return new List<string>()
            {
                "FL"
            };
        }
    }
}
