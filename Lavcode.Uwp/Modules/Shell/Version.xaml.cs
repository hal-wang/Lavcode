using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class Version : UserControl
    {
        public string VersionStr { get; } = Global.Version;

        public Version()
        {
            this.InitializeComponent();
        }
    }
}
