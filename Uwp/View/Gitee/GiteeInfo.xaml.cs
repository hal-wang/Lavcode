using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.View.Gitee
{
    public sealed partial class GiteeInfo : UserControl
    {
        public GiteeInfo()
        {
            this.InitializeComponent();

            Loaded += GiteeInfo_Loaded;
        }

        private async void GiteeInfo_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Model.Init();
        }


        public async void HandleViewSourceCode()
        {
            await Launcher.LaunchUriAsync(new Uri(Global.ReposUrl));
        }
    }
}
