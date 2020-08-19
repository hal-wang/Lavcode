using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Hubery.Lavcode.Uwp.Converters
{
    internal class ThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var size = (double)value;
            if (size <= 0)
            {
                return Convert(0, targetType);
            }

            var vals = (parameter as string).Split(',');
            if (vals.Length == 1)
            {
                return Convert(double.Parse(vals[0]) * size, targetType);
            }
            else if (vals.Length == 4)
            {
                return Convert(new double[]
                {
                    double.Parse(vals[0]) * size, double.Parse(vals[1]) * size, double.Parse(vals[2]) * size, double.Parse(vals[3]) * size
                }, targetType);
            }
            else
            {
                throw new NotSupportedException(parameter as string);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private object Convert(double size, Type targetType)
        {
            if (targetType == typeof(CornerRadius))
            {
                return new CornerRadius(size);
            }
            else if (targetType == typeof(Thickness))
            {
                return new Thickness(size);
            }
            else
            {
                throw new NotSupportedException(targetType.ToString());
            }
        }

        private object Convert(double[] sizes, Type targetType)
        {
            if (targetType == typeof(CornerRadius))
            {
                return new CornerRadius(sizes[0], sizes[1], sizes[2], sizes[3]);
            }
            else if (targetType == typeof(Thickness))
            {
                return new Thickness(sizes[0], sizes[1], sizes[2], sizes[3]);
            }
            else
            {
                throw new NotSupportedException(targetType.ToString());
            }
        }
    }
}
