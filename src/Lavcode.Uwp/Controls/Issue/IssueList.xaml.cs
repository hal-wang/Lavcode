using Lavcode.Common;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Controls.Issue
{
    public sealed partial class IssueList : UserControl
    {
        public IssueList()
        {
            this.InitializeComponent();
        }

        public IEnumerable<Octokit.Issue> Issues
        {
            get { return (IEnumerable<Octokit.Issue>)GetValue(IssuesProperty); }
            set { SetValue(IssuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Issues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IssuesProperty =
            DependencyProperty.Register("Issues", typeof(IEnumerable<Octokit.Issue>), typeof(IssueList), new PropertyMetadata(null));

        private async void MarkdownText_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private async void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var issue = (sender as FrameworkElement).DataContext as Octokit.Issue;
            await Launcher.LaunchUriAsync(new Uri($"{RepositoryConstant.GitHubUrl}/{issue.User.Login}"));
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is not Octokit.Issue issue)
            {
                return;
            }

            await Launcher.LaunchUriAsync(new Uri(issue.HtmlUrl));
        }
    }
}
