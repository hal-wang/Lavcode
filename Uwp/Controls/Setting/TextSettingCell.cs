using Windows.UI.Xaml;

namespace Hubery.Lavcode.Uwp.Controls.Setting
{
    public class TextSettingCell : BaseSettingCell
    {


        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(TextSettingCell), new PropertyMetadata(string.Empty));




    }
}
