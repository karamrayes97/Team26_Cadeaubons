using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cadeaubons_Presentation.Helpers
{
    public static class MessageHelper
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(
                message,
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        public static void ShowInfo(string message)
        {
            MessageBox.Show(
                message,
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}