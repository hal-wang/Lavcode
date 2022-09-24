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
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested -= this.OnCloseRequest;
            Requests.Clear();

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += this.OnCloseRequest;
        }

        private async void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            e.Handled = true;

            int level = -1;
            for (var i = 0; i < Requests.Count; i++)
            {
                if (!Requests.Any(req => req.level > level))
                {
                    break;
                }
                level = Requests.Where(req => req.level > level).Min(req => req.level);
                var request = Requests.First(req => req.level == level);
                if (!await request.Func())
                {
                    return;
                }
            }
            Application.Current.Exit();
        }

        public void Add(Func<Task<bool>> func, int level)
        {
            Requests.Add(new(func, level));
        }

        public void Remove(Func<Task<bool>> func)
        {
            Requests.RemoveAll(item => item.Func == func);
        }

        /// <summary>
        /// true: 执行后面的回调
        /// false: 不退出并且不执行后面的回调
        /// </summary>
        private List<(Func<Task<bool>> Func, int level)> Requests { get; } = new();
    }
}
