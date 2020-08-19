using GalaSoft.MvvmLight;
using Hubery.Lavcode.Uwp.Controls.Comment;
using Hubery.Lavcode.Uwp.Helpers;
using Hubery.Lavcode.Uwp.Model.Api;
using Hubery.Yt.Uwp.Helpers;
using Microsoft.Toolkit.Uwp;
using System;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.View.Feedback
{
    class FeedbackViewModel : ViewModelBase
    {
        private Issue _issue = null;
        public Issue Issue
        {
            get { return _issue; }
            set { Set(ref _issue, value); }
        }

        private IncrementalLoadingCollection<CommentSource, Comment> _feedbacks = null;
        public IncrementalLoadingCollection<CommentSource, Comment> Feedbacks
        {
            get { return _feedbacks; }
            set { Set(ref _feedbacks, value); }
        }

        public async void HandleRefresh()
        {
            Issue = null;
            await Init();
        }

        public async Task Init()
        {
            if (Issue != null)
            {
                return;
            }

            LoadingHelper.Show();
            await TaskExtend.SleepAsync(100);
            try
            {
                await GetIssueInfo();

                Feedbacks = new IncrementalLoadingCollection<CommentSource, Comment>(new CommentSource(Global.FeedbackIssueId));
                Feedbacks.OnEndLoading += () =>
                {
                    LoadingHelper.Hide();
                };
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                MessageHelper.ShowError(ex, 0);
            }
        }

        private async Task GetIssueInfo()
        {
            Issue = await ApiExtendHelper.GetIssue(Global.FeedbackIssueId);
        }

        public async void HandleFeedback()
        {
            var fbDialog = new FeedbackDialog();
            if (await fbDialog.ShowAsync() != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }

            if (Feedbacks == null || Feedbacks.HasMoreItems)
            {
                return;
            }

            Feedbacks.Insert(0, fbDialog.CommentResult);
            Issue.Comments++;
            RaisePropertyChanged(nameof(Issue));
        }
    }
}
