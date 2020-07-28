using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WindowsNOAAWidget.Services
{
    public static class ErrorHelper
    {
        public static TextBox ErrorLabel;

        public static void EmitError(string message)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ErrorLabel.Text = message;
            }));
        }

        public static void EmitError(Exception ex)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ErrorLabel.Text = ex.Message + Environment.NewLine + ex.StackTrace;
            }));
        }
    }
}
