using Hubery.Lavcode.Uwp.Helpers;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Hubery.Lavcode.Uwp.Converters
{
    internal class BackColorToForeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Color? defaultBackColor;
            if (string.IsNullOrEmpty(parameter as string))
            {
                defaultBackColor = Helpers.ColorHelper.GetBackColor();
            }
            else
            {
                defaultBackColor = Helpers.ColorHelper.GetColor(parameter as string);
            }

            Color? backColor = null;
            if (value == null)
            {
                backColor = null;
            }
            else if (value.GetType() == typeof(Color?))
            {
                backColor = (Color?)value;
            }
            else if (value.GetType() == typeof(Color))
            {
                backColor = (Color)value;
            }
            else if (value.GetType() == typeof(SolidColorBrush))
            {
                if (value is SolidColorBrush scb)
                {
                    var color = scb.Color;
                    color.A = (byte)(color.A * scb.Opacity);
                    backColor = color;
                }
            }
            else
            {
                var color = value.ToString();
                if (color != null && color.Length > 0 && color[0] == '#')
                {
                    backColor = Helpers.ColorHelper.GetColor(color);
                }
                else
                {
                    backColor = ResourcesHelper.GetResource<Color>(color);
                }
            }

            bool isBackDark;
            if (backColor == null)
            {
                isBackDark = ThemeHelper.ElementTheme == Windows.UI.Xaml.ElementTheme.Dark;
            }
            else
            {
                isBackDark = Helpers.ColorHelper.IsDarkColor(backColor.Value, backColor: defaultBackColor);
            }

            if (targetType == typeof(Color))
            {
                return isBackDark ? Colors.White : Colors.Black;
            }
            else if (targetType == typeof(SolidColorBrush) || targetType == typeof(Brush))
            {
                return isBackDark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
