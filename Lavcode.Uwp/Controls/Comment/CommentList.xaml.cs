using Octokit;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.Controls.Comment
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
    }
}
