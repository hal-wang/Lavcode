using System;
using Windows.UI.Xaml.Data;

namespace Hubery.Lavcode.Uwp.Converters
{
    internal class IsEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isEqual;
            if (value.GetType() == parameter.GetType())
            {
                isEqual = value == parameter;
            }
            else
            {
                isEqual = value.ToString() == parameter.ToString();
            }

            return ConverterHelper.BoolTo(isEqual, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
