using System;
using Windows.UI.Xaml.Data;

namespace Hubery.Lavcode.Uwp.Converters
{
    /// <summary>
    /// 末尾有L表示Local
    /// </summary>
    internal class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ConverterHelper.TimeToStr(value, parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateTime.ParseExact(value as string, parameter as string, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}