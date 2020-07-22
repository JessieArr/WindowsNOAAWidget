using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WindowsNOAAWidget.Services;

namespace WindowsNOAAWidget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string _LogName = "crash-report.log";
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                if (ex != null)
                {
                    File.WriteAllText(_LogName, $"{ex.Message}:{Environment.NewLine}{ex.StackTrace}");
                }
                else
                {
                    File.WriteAllText(_LogName, $"Exception not found.");
                }
            };
        }
    }
}
