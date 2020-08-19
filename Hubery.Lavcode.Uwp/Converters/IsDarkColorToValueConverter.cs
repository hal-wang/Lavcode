using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Hubery.Lavcode.Uwp.Converters
{
    internal class IsDarkColorToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Color color;
            if (value is Color)
            {
                color = (Color)value;
            }
            else if (value is SolidColorBrush scb)
            {
                var c = scb.Color;
                c.A = (byte)(c.A * scb.Opacity);
                color = c;
            }
            else if (value is string str)
            {
                color = Helpers.ColorHelper.GetColor(str);
            }
            else
            {
                throw new NotSupportedException();
            }

            return ConverterHelper.BoolToValue(value, Helpers.ColorHelper.IsDarkColor(color), targetType, parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
