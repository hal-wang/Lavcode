using System;
using Windows.UI.Xaml.Data;

namespace Hubery.Lavcode.Uwp.Converters
{
    public class DateParseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var parameters = (parameter as string).Split(':', 2);

            var dateTime = DateTime.ParseExact(value as string, parameters[0], System.Globalization.CultureInfo.InvariantCulture);
            return ConverterHelper.TimeToStr(dateTime, parameters[1]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
