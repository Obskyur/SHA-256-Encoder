using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHA_256_Encoder.Controllers
{
    internal static class Dialog
    {
        public static async Task ShowAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = MainWindow.Current.Content.XamlRoot,
                Title = title,
                Content = message,
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }
    }
}
