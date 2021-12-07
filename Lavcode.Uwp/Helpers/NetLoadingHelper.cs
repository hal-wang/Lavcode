using HTools.Uwp.Helpers;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Helpers
{
    public static class NetLoadingHelper
    {
        public static async Task<T> Invoke<T>(Func<Task<T>> func, string loadingStr = "")
        {
            if (Global.IsNetworked)
            {
                LoadingHelper.Show(loadingStr);
            }
            try
            {
                return await func();
            }
            finally
            {
                if (Global.IsNetworked)
                {
                    LoadingHelper.Hide();
                }
            }
        }

        public static async Task Invoke(Func<Task> func, string loadingStr = "")
        {
            await Invoke(async () => await func(), loadingStr);
        }
    }
}
