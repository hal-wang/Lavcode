using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.SqliteSync;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class Version : UserControl
    {
        public string VersionStr => Global.Version;

        private bool IsLaunchFile => ServiceProvider.Services.GetService<SqliteFileService>()?.OpenedFile != null;
        private Provider Provider => SettingHelper.Instance.Provider;
        private bool IsProviderVisible => ProviderVisibility == Visibility.Visible && !IsLaunchFile;

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
