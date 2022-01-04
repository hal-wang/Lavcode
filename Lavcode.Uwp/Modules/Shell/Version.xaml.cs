using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class Version : UserControl
    {
        public string VersionStr => Global.Version;

        public Provider Provider => SettingHelper.Instance.Provider;




        public Visibility ProviderVisibility
        {
            get { return (Visibility)GetValue(ProviderVisibilityProperty); }
            set { SetValue(ProviderVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProviderVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProviderVisibilityProperty =
            DependencyProperty.Register("ProviderVisibility", typeof(Visibility), typeof(Version), new PropertyMetadata(Visibility.Visible));




        public Version()
        {
            this.InitializeComponent();
        }
    }
}
