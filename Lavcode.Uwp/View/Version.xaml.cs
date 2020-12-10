using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.View
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
