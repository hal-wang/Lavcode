using Windows.UI.Xaml.Controls;

namespace Lavcode.View
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
