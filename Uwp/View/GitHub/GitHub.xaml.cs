using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.View.GitHub
{
    public sealed partial class GitHub : UserControl
    {
        public GitHub()
        {
            this.InitializeComponent();

            Loaded += GitHub_Loaded;
        }

        private async void GitHub_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Model.Init();
        }


        public async void HandleViewSourceCode()
        {
            await Launcher.LaunchUriAsync(new Uri(Global.ReposUrl));
        }
    }
}
