using Lavcode.Common;
using Octokit;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Controls.Comment
{
    public sealed partial class CommentList : UserControl
    {
        public CommentList()
        {
            this.InitializeComponent();
        }

        public IEnumerable<IssueComment> Comments
        {
            get { return (IEnumerable<IssueComment>)GetValue(CommentsProperty); }
            set { SetValue(CommentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Comments.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentsProperty =
            DependencyProperty.Register("Comments", typeof(IEnumerable<IssueComment>), typeof(CommentList), new PropertyMetadata(null));



        public bool IsAuthorVisible
        {
            get { return (bool)GetValue(IsAuthorVisibleProperty); }
            set { SetValue(IsAuthorVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAuthorVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAuthorVisibleProperty =
            DependencyProperty.Register("IsAuthorVisible", typeof(bool), typeof(CommentList), new PropertyMetadata(true));

        private async void MarkdownText_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private async void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var comment = (sender as FrameworkElement).DataContext as IssueComment;
            await Launcher.LaunchUriAsync(new Uri($"{RepositoryConstant.GitHubUrl}/{comment.User.Login}"));
        }
    }
}
