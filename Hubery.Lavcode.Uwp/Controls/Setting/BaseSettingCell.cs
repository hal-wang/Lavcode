using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.Controls.Setting
{
    /// <summary>
    /// 为什么不用泛型：UWP暂不支持 x:TypeArguments
    /// </summary>
    public class BaseSettingCell : ContentControl
    {
        public BaseSettingCell()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(BaseSettingCell), new PropertyMetadata(string.Empty));




        public Visibility NextVisible
        {
            get { return (Visibility)GetValue(NextVisibleProperty); }
            set { SetValue(NextVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextVisibleProperty =
            DependencyProperty.Register("NextVisible", typeof(Visibility), typeof(BaseSettingCell), new PropertyMetadata(Visibility.Visible));


    }
}
