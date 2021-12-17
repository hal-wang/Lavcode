using HTools;
using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.View.Sync.SyncHelper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.SqliteSync.View
{
    public sealed partial class SyncFileHandler : UserControl
    {
        public event TypedEventHandler<SyncFileHandler, StorageFile> OnLoaded;

        public SyncFileHandler()
        {
            this.InitializeComponent();
            Loaded += SyncFileHandler_Loaded;
            Unloaded += SyncFileHandler_Unloaded;
            Init();
        }

        private void SyncFileHandler_Unloaded(object sender, RoutedEventArgs e)
        {
            ExitHandler.Instance.Remove(OnCloseRequest);
        }

        private void SyncFileHandler_Loaded(object sender, RoutedEventArgs e)
        {
            ExitHandler.Instance.Add(OnCloseRequest, 1);
        }

        private async Task<StorageFile> GetTempFile()
        {
            var sfs = ServiceProvider.Services.GetService<SqliteFileService>();
            var launchFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(sfs.FileLaunchFolderName, CreationCollisionOption.OpenIfExists);
            return await launchFolder.GetFileAsync(sfs.FileLaunchFileName);
        }

        private async Task<bool> OnCloseRequest()
        {
            if (OpenedFile == null)
            {
                return true;
            }

            if (IsDbChanged)
            {
                try
                {
                    // 显显示编辑未保存对话框。如果未编辑，则关闭程序。
                    var cdr = await PopupHelper.ShowDialog("当前备份文件已经被编辑，但未保存，确认退出？", "文件未保存", "保存并退出", "不保存退出", null, true, "点错了");
                    if ((cdr == ContentDialogResult.Primary && await UpdateToFile()) || cdr == ContentDialogResult.Secondary)
                    {
                        await Clean();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError(ex);
                    return false;
                }
            }
            else
            {
                await Clean();
                return true;
            }
        }

        private async Task Clean()
        {
            LoadingHelper.Show("正在清理");
            await TaskExtend.SleepAsync();
            try
            {
                var localTempFile = await GetTempFile();
                await localTempFile.DeleteAsync();
            }
            catch { }
        }

        public bool IsDbChanged
        {
            get { return (bool)GetValue(IsDbChangedProperty); }
            set { SetValue(IsDbChangedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDbChanged.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDbChangedProperty =
            DependencyProperty.Register("IsDbChanged", typeof(bool), typeof(SyncFileHandler), new PropertyMetadata(false));

        public StorageFile OpenedFile
        {
            get { return (StorageFile)GetValue(OpenedFileProperty); }
            set { SetValue(OpenedFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpenedFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenedFileProperty =
            DependencyProperty.Register("OpenedFile", typeof(StorageFile), typeof(SyncFileHandler), new PropertyMetadata(null));


        private async void Init()
        {
            await TaskExtend.SleepAsync(100);
            var sfs = ServiceProvider.Services.GetService<SqliteFileService>();
            OpenedFile = sfs.OpenedFile;
            if (sfs.SyncHelper == null)
            {
                sfs.SyncHelper = await FileSyncHelper.Create(OpenedFile);
                if (sfs.SyncHelper == null)
                {
                    var dispatcherTimer = new DispatcherTimer()
                    {
                        Interval = new TimeSpan(0, 0, 5)
                    };
                    dispatcherTimer.Tick += (ss, ee) => Application.Current.Exit();
                    MessageHelper.ShowDanger("文件打开失败\n窗口将在5秒后自动关闭", 0);
                    dispatcherTimer.Start();
                    return;
                }

                var launchFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(sfs.FileLaunchFolderName, CreationCollisionOption.OpenIfExists);
                await ServiceProvider.Services.GetService<IConService>().Connect(new { FilePath = Path.Combine(launchFolder.Path, sfs.FileLaunchFileName) });
                (ServiceProvider.Services.GetService<IConService>() as Service.Sqlite.ConService).Connection.TableChanged += async (ss, ee) =>
                     await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => IsDbChanged = true);

                if (sfs.SyncHelper.IsAutoVerified)
                {
                    MessageHelper.ShowPrimary("密码正确，自动打开", 5000);
                }
            }
            OnLoaded?.Invoke(this, OpenedFile);
        }

        private async void UpdateToFile_Click(object sender, RoutedEventArgs e)
        {
            LoadingHelper.Show();
            await TaskExtend.SleepAsync();
            try
            {
                if (!await UpdateToFile())
                {
                    return;
                }

                MessageHelper.ShowPrimary("保存完成");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
                return;
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAnother">是否另存为</param>
        /// <returns></returns>
        private async Task<bool> UpdateToFile(bool isAnother = false)
        {
            var localTempFile = await GetTempFile();

            var sfs = ServiceProvider.Services.GetService<SqliteFileService>();
            var result = isAnother ? await sfs.SyncHelper.SetFile(localTempFile, file =>
                {
                    var oldFile = OpenedFile;
                    sfs.OpenedFile = file;
                    OpenedFile = file;
                })
                : await sfs.SyncHelper.SetFile(localTempFile);
            if (result)
            {
                IsDbChanged = false;
            }
            return result;
        }

        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            LoadingHelper.Show();
            await TaskExtend.SleepAsync();
            try
            {
                if (!await UpdateToFile(true))
                {
                    return;
                }

                MessageHelper.ShowPrimary("保存完成");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
                return;
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }
    }
}
