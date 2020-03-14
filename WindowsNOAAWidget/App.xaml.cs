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
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                File.WriteAllText("crash-report.log", $"{ex.Message}:{Environment.NewLine}{ex.StackTrace}");
                ErrorHelper.EmitError(ex);
            };

            AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            {
                var ex = args.Exception;
                ErrorHelper.EmitError(ex);
            };
        }
    }
}
