using Lavcode.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class BackSvg : UserControl
    {
        public BackSvg()
        {
            this.InitializeComponent();

            Loaded += (s, e) => SettingHelper.Instance.IsBgVisibleChanged += OnIsBgVisibleChanged;
            Unloaded += (s, e) => SettingHelper.Instance.IsBgVisibleChanged -= OnIsBgVisibleChanged;
        }

        private void OnIsBgVisibleChanged(bool imgVisible) => ImgVisible = imgVisible;

        public bool ImgVisible
        {
            get { return (bool)GetValue(ImgVisibleProperty); }
            set { SetValue(ImgVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImgVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImgVisibleProperty =
            DependencyProperty.Register("ImgVisible", typeof(bool), typeof(BackSvg), new PropertyMetadata(SettingHelper.Instance.IsBgVisible));
    }
}
