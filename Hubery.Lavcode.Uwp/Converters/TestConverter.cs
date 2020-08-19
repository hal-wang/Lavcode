using Newtonsoft.Json;
using System;
using Windows.UI.Xaml.Data;

namespace Hubery.Lavcode.Uwp.Converters
{
    /// <summary>
    /// 用于绑定测试
    /// </summary>
    class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Console.WriteLine("TestConverter Convert");
            Console.WriteLine("value: " + JsonConvert.SerializeObject(value));
            Console.WriteLine("targetType: " + targetType);
            Console.WriteLine("parameter: " + parameter);
            Console.WriteLine("language: " + language);

            return GetDefault(targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Console.WriteLine("TestConverter ConvertBack");
            Console.WriteLine("value: " + JsonConvert.SerializeObject(value));
            Console.WriteLine("targetType: " + targetType);
            Console.WriteLine("parameter: " + parameter);
            Console.WriteLine("language: " + language);

            return GetDefault(targetType);
        }

        private object GetDefault(Type targetType)
        {
            if (targetType.IsValueType)
            {
                return Activator.CreateInstance(targetType);
            }
            else
            {
                return null;
            }
        }
    }
}
