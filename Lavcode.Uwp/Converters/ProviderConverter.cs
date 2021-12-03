using Lavcode.Model;
using System;
using Windows.UI.Xaml.Data;

namespace Lavcode.Uwp.Converters
{
    internal class ProviderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not Provider provider) return "";
            return provider switch
            {
                Provider.Sqlite => "本地",
                Provider.GitHub => "GitHub",
                _ => "",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
