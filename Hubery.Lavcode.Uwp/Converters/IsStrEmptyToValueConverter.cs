using System;
using Windows.UI.Xaml.Data;

namespace Hubery.Lavcode.Uwp.Converters
{
    internal class IsStrEmptyToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConverterHelper.BoolToValue(value, string.IsNullOrEmpty(value as string), targetType, parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
