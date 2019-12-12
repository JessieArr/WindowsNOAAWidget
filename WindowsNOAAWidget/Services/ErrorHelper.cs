using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WindowsNOAAWidget.Services
{
    public static class ErrorHelper
    {
        public static Label ErrorLabel;

        public static void EmitError(string message)
        {
            Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
            {
                ErrorLabel.Content = message;
            }));
        }
    }
}
