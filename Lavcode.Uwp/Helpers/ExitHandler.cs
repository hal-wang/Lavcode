using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;

namespace Lavcode.Uwp.Helpers
{
    public class ExitHandler
    {
        private ExitHandler() { }
        public static ExitHandler Instance { get; } = new ExitHandler();

        public void Register()
        {
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += this.OnCloseRequest;
        }

        public void Unregister()
        {
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested -= this.OnCloseRequest;
            Requests.Clear();
        }

        private async void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            e.Handled = true;
            for (var i = 0; i < Requests.Count; i++)
            {
                var level = Requests.Min(req => req.level);
                var request = Requests.First(req => req.level == level);
                if (!await request.Func())
                {
                    return;
                }
            }
            Application.Current.Exit();
        }

        /// <summary>
        /// true: 执行后面的回调
        /// false: 不退出并且不执行后面的回调
        /// </summary>
        public List<(Func<Task<bool>> Func, int level)> Requests { get; } = new();
    }
}
