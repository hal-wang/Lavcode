using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Hubery.Lavcode.Uwp.Converters
{
    internal class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConverterHelper.BoolTo(value == null, targetType, parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}