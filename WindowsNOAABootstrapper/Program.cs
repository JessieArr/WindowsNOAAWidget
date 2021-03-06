﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var widgetPath = Path.Combine(applicationPath, "WindowsNOAAWidget.exe");
            Process.Start(widgetPath);
        }
    }
}
