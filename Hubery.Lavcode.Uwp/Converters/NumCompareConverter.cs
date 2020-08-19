using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Hubery.Lavcode.Uwp.Converters
{
    internal class NumCompareConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string param = parameter as string;
            var isTrue = (param[0]) switch
            {
                '>' => (int)value > int.Parse(param[1].ToString()),
                '<' => (int)value < int.Parse(param[1].ToString()),
                '=' => (int)value == int.Parse(param[1].ToString()),
                _ => throw new Exception(),
            };

            return ConverterHelper.BoolTo(isTrue, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}