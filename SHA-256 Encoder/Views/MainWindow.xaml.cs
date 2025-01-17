using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using SHA_256_Encoder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SHA_256_Encoder
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private Models.App App { get; set; } = new Models.App();
        private BoolToBrushConverter BoolToBrushConverter { get; set; } = new BoolToBrushConverter();
        public MainWindow()
        {
            this.InitializeComponent();
            this.Width = 400;
            this.Height = 400;
            this.authIcon.Fill = (RadialGradientBrush)BoolToBrushConverter.Convert(
                App.State.IsAuthenticated, 
                typeof(RadialGradientBrush), 
                null, 
                "");
            this.verifiedIcon.Fill = (RadialGradientBrush)BoolToBrushConverter.Convert(
                App.State.IsVerified, 
                typeof(RadialGradientBrush), 
                null, 
                "");
        }

        public double Width
        {
            get { return this.Bounds.Width; }
            set { this.AppWindow.Resize(new Windows.Graphics.SizeInt32((int)value, (int)this.Height)); }
        }

        public double Height
        {
            get { return this.Bounds.Height; }
            set { this.AppWindow.Resize(new Windows.Graphics.SizeInt32((int)this.Width, (int)value)); }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            submitButton.Content = "Clicked";
        }
    }
}
