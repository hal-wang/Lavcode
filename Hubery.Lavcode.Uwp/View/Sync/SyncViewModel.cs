using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Hubery.Lavcode.Uwp.View.Sync.SyncHelper;
using Hubery.Tools;
using Hubery.Tools.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hubery.Lavcode.Uwp.View.Sync
{
    public class SyncViewModel : ViewModelBase
    {
        #region 导出
        private async Task LocalTo(bool isRemote)
        {
            try
            {
                LoadingHelper.Show($"正在{(isRemote ? "上传" : "导出")}");
                await TaskExtend.SleepAsync();

                var syncHelper = isRemote ? await DavSyncHelper.Create() : FileSyncHelper.Create();
                if (syncHelper == null)
                {
                    return;
                }

                if (await syncHelper.SetFile(await syncHelper.GetTempLocalFile()))
                {
                    MessageHelper.ShowPrimary("备份完成");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }

        /// <summary>
        /// 本地覆盖云端
        /// </summary>
        public async void HandleLocalToRemote()
        {
            await LocalTo(true);
        }

        public async void HandleLocalToFile()
        {
            await LocalTo(false);
        }
        #endregion

        #region 导入
        private async Task ToLocal(bool isRemote)
        {
            try
            {
                LoadingHelper.Show(isRemote ? "正在下载" : "获取文件");
                await TaskExtend.SleepAsync();

                var syncHelper = isRemote ? await DavSyncHelper.Create() : FileSyncHelper.Create();
                if (syncHelper == null)
                {
                    return;
                }
                var remoteDbFile = await syncHelper.GetFile();
                if (remoteDbFile == null)
                {
                    return;
                }
                LoadingHelper.Show("正在整理");
                await syncHelper.ReplaceDbFile(remoteDbFile);

                Messenger.Default.Send<object>(null, "OnDbRecovered");
                MessageHelper.ShowPrimary("恢复完成");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }
        /// <summary>
        /// 云端覆盖本地
        /// </summary>
        public async void HandleRemoteToLocal()
        {
            await ToLocal(true);
        }
        public async void HandleFileToLocal()
        {
            await ToLocal(false);
        }
        #endregion

        #region 合并
        private async Task Merge(bool isRemote)
        {
            try
            {
                LoadingHelper.Show(isRemote ? "正在下载" : "获取文件");
                await TaskExtend.SleepAsync();

                var syncHelper = isRemote ? await DavSyncHelper.Create() : FileSyncHelper.Create();
                if (syncHelper == null)
                {
                    return;
                }
                var remoteDbFile = await syncHelper.GetFile();
                if (remoteDbFile == null)
                {
                    return;
                }

                LoadingHelper.Show("正在整理");
                var localDbFile = await syncHelper.GetTempLocalFile();
                using (Merge merge = new Merge(localDbFile.Path, remoteDbFile.Path))
                {
                    await merge.AutoMerge();
                }

                LoadingHelper.Show($"正在{(isRemote ? "上传" : "导出")}");
                if (!await syncHelper.SetFile(localDbFile))
                {
                    return;
                }
                await syncHelper.ReplaceDbFile(localDbFile);

                Messenger.Default.Send<object>(null, "OnDbRecovered");
                MessageHelper.ShowPrimary("同步完成");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }

        /// <summary>
        /// 本地与云端合并
        /// </summary>
        public async void HandleRemoteMerge()
        {
            await Merge(true);
        }

        public async void HandleFileMerge()
        {
            await Merge(false);
        }
        #endregion

        #region Cleanup
        public async static Task CleanupTempFile()
        {
            try
            {
                var tempFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(Global.SyncTempFolderName);
                await tempFolder.DeleteAsync();
            }
            catch { }
        }
        #endregion
    }
}