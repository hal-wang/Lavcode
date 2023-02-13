using HTools;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Services.Store;

namespace Lavcode.Uwp.Helpers
{
    public static class UpdateHelper
    {
        public static async Task<bool> DownloadAndInstallAllUpdatesAsync()
        {
            var result = false;
            await TaskExtend.Run(async () =>
            {
                try
                {
                    var context = StoreContext.GetDefault();
                    var updates = await context.GetAppAndOptionalStorePackageUpdatesAsync();
                    if (updates.Count > 0)
                    {
                        var downloadResult = await context.RequestDownloadStorePackageUpdatesAsync(updates);
                        if (downloadResult.OverallState == StorePackageUpdateState.Completed)
                        {
                            await context.RequestDownloadAndInstallStorePackageUpdatesAsync(updates);
                        }
                    }
                    result = updates.Count > 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    result = false;
                }
            });
            return result;
        }

        public static async void DownloadAndInstallAllUpdatesBackground()
        {
            await DownloadAndInstallAllUpdatesAsync();
        }
    }
}
