using Windows.UI.Xaml;

namespace Hubery.Lavcode.Uwp.Controls.Setting
{
    public class BoolSettingCell : BaseSettingCell
    {
        public bool Value
        {
            get { return (bool)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(bool), typeof(BaseSettingCell), new PropertyMetadata(false));
    }
}
