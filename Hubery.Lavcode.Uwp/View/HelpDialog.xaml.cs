using Hubery.Lavcode.Uwp.Controls.Dialog;
using System;
using Windows.System;
using Windows.UI.Xaml;

namespace Hubery.Lavcode.Uwp.View
{
    public sealed partial class HelpDialog : LayoutDialog
    {
        public HelpDialog()
        {
            this.InitializeComponent();
        }

        public string ReposUrl => Global.ReposUrl;

        public string AES256Url => "https://baike.baidu.com/item/高级加密标准";

        private void Donate_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DonateFlyout.ShowAt(sender as FrameworkElement);
            FindName(nameof(Rating));
        }

        private async void MoreHelp_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(Global.HomeUrl));
        }
    }
}
