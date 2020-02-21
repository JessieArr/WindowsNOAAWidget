using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
            var myArgs = Environment.GetCommandLineArgs();
            if(myArgs.Count() < 2)
            {
                // This is a workaround for an issue on Windows. If an application is started from a shortcut,
                // the shortcut's icon is used, preventing updating ours dynamically at runtime. So by doing 
                // this, we are able to start a second copy of the application which is not started from a shortcut,
                // giving us back control over our own icon in the Windows taskbar.
                Process.Start(myArgs[0], "chain");
                Application.Current.Shutdown();
            }
            else
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    var ex = args.ExceptionObject as Exception;
                    ErrorHelper.EmitError(ex.Message);
                };

                AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
                {
                    var ex = args.Exception;
                    ErrorHelper.EmitError(ex.Message);
                };
            }            
        }
    }
}
