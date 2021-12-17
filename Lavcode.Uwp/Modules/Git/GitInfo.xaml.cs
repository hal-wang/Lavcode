using Lavcode.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Git
{
    public sealed partial class GitInfo : UserControl
    {
        public GitInfo()
        {
            DataContext = VM;
            this.InitializeComponent();

            Loaded += GitInfo_Loaded;
        }

        public GitInfoViewModel VM { get; } = ServiceProvider.Services.GetService<GitInfoViewModel>();

        private async void GitInfo_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await VM.Init();
        }


        public async void HandleViewSourceCode()
        {
            await Launcher.LaunchUriAsync(new Uri(RepositoryConstant.GitHubRepositoryUrl));
        }
    }
}
