using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Shell
{
    public sealed partial class Version : UserControl
    {
        public string VersionStr => Global.Version + " Beta";

        public Version()
        {
            this.InitializeComponent();
        }
    }
}
