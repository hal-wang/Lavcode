using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.View.Git
{
    public sealed partial class GitInfo : UserControl
    {
        public GitInfo()
        {
            this.InitializeComponent();

            Loaded += GitInfo_Loaded;
        }

        private async void GitInfo_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Model.Init();
        }


        public async void HandleViewSourceCode()
        {
            await Launcher.LaunchUriAsync(new Uri(Global.ReposUrl));
        }
    }
}
