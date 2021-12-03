using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class Version : UserControl
    {
        public string VersionStr { get; } = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";

        public Version()
        {
            this.InitializeComponent();
        }
    }
}
