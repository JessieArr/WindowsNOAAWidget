using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsNOAABootstrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            // This is a workaround for an issue where Windows uses a shortcut's icon for an application when it
            // is pinned to the taskbar. We can pin this application and use it to start the NOAA Widget without
            // having our icon overridden by a Windows shortcut.
            Process.Start("WindowsNOAAWidget.exe");
        }
    }
}
