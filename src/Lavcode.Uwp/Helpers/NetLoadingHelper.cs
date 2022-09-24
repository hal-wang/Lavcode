using HTools.Uwp.Helpers;
using System;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Helpers
{
    public static class NetLoadingHelper
    {
        public static async Task<T> Invoke<T>(Func<Task<T>> func, string loadingStr = "", bool exMsg = true)
        {
            if (Global.IsNetworked)
            {
                LoadingHelper.Show(loadingStr);
            }
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                if (exMsg)
                {
                    MessageHelper.ShowError(ex);
                    return default;
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (Global.IsNetworked)
                {
                    LoadingHelper.Hide();
                }
            }
        }

        public static async Task Invoke(Func<Task> func, string loadingStr = "", bool exMsg = true)
        {
            await Invoke<object>(async () =>
            {
                await func();
                return null;
            }, loadingStr, exMsg);
        }
    }
}
