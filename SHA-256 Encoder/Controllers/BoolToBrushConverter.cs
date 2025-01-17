using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace SHA_256_Encoder
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;
            var gradientBrush = new RadialGradientBrush();
            gradientBrush.GradientStops.Add(new GradientStop
            {
                Color = boolValue ? Microsoft.UI.Colors.Green : Microsoft.UI.Colors.Red,
                Offset = 0.0
            });
            gradientBrush.GradientStops.Add(new GradientStop
            {
                Color = Microsoft.UI.Colors.Transparent,
                Offset = 1.0
            });
            return gradientBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
