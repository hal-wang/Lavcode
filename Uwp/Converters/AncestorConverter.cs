using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Hubery.Lavcode.Uwp.Converters
{
    internal class AncestorConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">目标控件（Tag值 或 层数）:属性名</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            FrameworkElement element = value as FrameworkElement;

            if (int.TryParse(parameter as string, out int num))
            {
                for (int i = 0; i < num; i++)
                {
                    element = element.Parent as FrameworkElement;
                }
            }
            else
            {
                while (true)
                {
                    if (element.Tag as string == parameter as string)
                    {
                        return element.DataContext;
                    }

                    element = (FrameworkElement)VisualTreeHelper.GetParent(element);
                }
            }

            return element;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
