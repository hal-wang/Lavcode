using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Services.Store;

namespace Lavcode.Uwp.Helpers
{
    public static class UpdateHelper
    {
        public static async Task DownloadAndInstallAllUpdatesAsync()
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static async void DownloadAndInstallAllUpdates()
        {
            await DownloadAndInstallAllUpdatesAsync();
        }
    }
}
