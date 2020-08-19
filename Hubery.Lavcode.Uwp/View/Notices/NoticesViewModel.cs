using GalaSoft.MvvmLight;
using Hubery.Lavcode.Uwp.Controls.Comment;
using Hubery.Lavcode.Uwp.Helpers;
using Hubery.Lavcode.Uwp.Model.Api;
using Hubery.Yt.Uwp.Helpers;
using Microsoft.Toolkit.Uwp;
using System;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.View.Notices
{
    public class NoticesViewModel : ViewModelBase
    {
        public string Author => Global.GiteeAccount;

        private Issue _issue = null;
        public Issue Issue
        {
            get { return _issue; }
            set { Set(ref _issue, value); }
        }

        private IncrementalLoadingCollection<CommentSource, Comment> _notices = null;
        public IncrementalLoadingCollection<CommentSource, Comment> Notices
        {
            get { return _notices; }
            set { Set(ref _notices, value); }
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

                Notices = new IncrementalLoadingCollection<CommentSource, Comment>(new CommentSource(Global.NoticeIssueId));
                Notices.OnEndLoading += () =>
                {
                    LoadingHelper.Hide();
                };
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex, 0);
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }

        private async Task GetIssueInfo()
        {
            Issue = await ApiExtendHelper.GetIssue(Global.NoticeIssueId);
        }
    }
}
