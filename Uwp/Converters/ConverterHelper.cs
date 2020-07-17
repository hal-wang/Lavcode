using Hubery.Lavcode.Uwp.Helpers;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Hubery.Lavcode.Uwp.Converters
{
    public static class ConverterHelper
    {
        public static object BoolToValue(object value, bool isTrue, Type targetType, string parameter)
        {
            string[] strs = parameter.Split(':');
            string value1 = strs[0];
            string value2 = strs[1];
            if (
                    (isTrue && string.Equals(value1, "@VALUE", StringComparison.InvariantCultureIgnoreCase)) ||
                    (!isTrue && string.Equals(value2, "@VALUE", StringComparison.InvariantCultureIgnoreCase))
                )
            {
                if (value.GetType() == targetType)
                {
                    return value;
                }

                if (!(value is string str))
                {
                    return value.GetValue(targetType);
                }

                if (isTrue)
                {
                    value1 = str;
                }
                else
                {
                    value2 = str;
                }
            }

            if (targetType == typeof(int))
            {
                return isTrue ? int.Parse(value1) : int.Parse(value2);
            }
            else if (targetType == typeof(double))
            {
                return isTrue ? double.Parse(value1) : double.Parse(value2);
            }
            else if (targetType == typeof(Windows.UI.Color))
            {
                return isTrue ? ColorHelper.GetColor(value1) : ColorHelper.GetColor(value2);
            }
            else if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
            {
                return isTrue ? ColorHelper.GetBrush(value1) : ColorHelper.GetBrush(value2);
            }
            else if (targetType == typeof(Thickness))
            {
                return isTrue ? (Thickness)TypeDescriptor.GetConverter(typeof(Thickness)).ConvertFrom(value1) : (Thickness)TypeDescriptor.GetConverter(typeof(Thickness)).ConvertFrom(value2);
            }
            else if (targetType.IsEnum)
            {
                return isTrue ? Enum.Parse(targetType, value1) : Enum.Parse(targetType, value2);
            }
            else
            {
                return isTrue ? value1 : value2;
            }
        }

        public static object BoolTo(bool isTrue, Type targetType, string parameter = null)
        {
            if (string.Equals(parameter, "true", StringComparison.InvariantCultureIgnoreCase) || string.Equals(parameter, "T", StringComparison.InvariantCultureIgnoreCase))
            {
                isTrue = !isTrue;
            }

            if (targetType == typeof(bool))
            {
                return isTrue;
            }
            else if (targetType == typeof(Visibility))
            {
                return isTrue ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static object TimeToStr(object value, string parameter)
        {
            if (value == null)
            {
                return default;
            }

            bool isLocal = false;
            if (!string.IsNullOrEmpty(parameter) && parameter[parameter.Length - 1] == 'L')
            {
                isLocal = true;
                parameter = parameter.Substring(0, parameter.Length - 1);
            }

            if (value is DateTime dateTime)
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    return dateTime.ToString();
                }
                else if (isLocal)
                {
                    return dateTime.ToLocalTime().ToString(parameter);
                }
                else
                {
                    return dateTime.ToString(parameter);
                }
            }
            else if (value is TimeSpan timeSpan)
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    return timeSpan.ToString();
                }
                else
                {
                    return timeSpan.ToString(parameter);
                }
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    return dateTimeOffset.ToString();
                }
                else if (isLocal)
                {
                    return dateTimeOffset.ToLocalTime().ToString(parameter);
                }
                else
                {
                    return dateTimeOffset.ToString(parameter);
                }
            }
            else if (value is string str)
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    return DateTime.Parse(str).ToString();
                }
                else if (isLocal)
                {
                    return DateTime.Parse(str).ToLocalTime().ToString(parameter);
                }
                else
                {
                    return DateTime.Parse(str).ToString(parameter);
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
