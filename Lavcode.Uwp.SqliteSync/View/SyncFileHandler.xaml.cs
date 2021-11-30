using GalaSoft.MvvmLight.Ioc;
using HTools;
using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Uwp.View.Sync.SyncHelper;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.SqliteSync.View
{
    public sealed partial class SyncFileHandler : UserControl
    {
        private ISyncHelper _syncHelper;
        public event TypedEventHandler<SyncFileHandler, StorageFile> OnLoaded;

        public SyncFileHandler()
        {
            this.InitializeComponent();

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += this.OnCloseRequest;
            Global.UnsaveDialogAction += this.ShowUnsaveDialog;
            Init();
        }

        ~SyncFileHandler()
        {
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested -= this.OnCloseRequest;
            Global.UnsaveDialogAction -= this.ShowUnsaveDialog;
        }

        private async Task<StorageFile> GetTempFile()
        {
            var launchFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(Global.FileLaunchFolderName, CreationCollisionOption.OpenIfExists);
            return await launchFolder.GetFileAsync(Global.FileLaunchFileName);
        }

        private async void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            if (OpenedFile == null)
            {
                return;
            }

            e.Handled = true;
            if (IsDbChanged)
            {
                ShowUnsaveDialog();
            }
            else
            {
                await CleanUp();
            }
        }

        /// <summary>
        /// 显显示编辑未保存对话框。
        /// 如果未编辑，则直接关闭程序。
        /// </summary>
        private async void ShowUnsaveDialog()
        {
            if (!IsDbChanged) // 如果能够调用到这个函数，但文件未编辑，则直接关闭程序
            {
                await CleanUp();
            }

            try
            {
                var cdr = await PopupHelper.ShowDialog("当前备份文件已经被编辑，但未保存，确认退出？", "文件未保存", "保存并退出", "不保存退出", null, true, "点错了");
                if ((cdr == ContentDialogResult.Primary && await UpdateToFile()) || cdr == ContentDialogResult.Secondary)
                {
                    await CleanUp();
                }
            }
            catch { }
        }

        private async Task CleanUp()
        {
            LoadingHelper.Show("正在清理");
            await TaskExtend.SleepAsync();
            try
            {
                var localTempFile = await GetTempFile();
                await localTempFile.DeleteAsync();
            }
            catch { }
            Application.Current.Exit();
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
            OpenedFile = Global.OpenedFile;
            _syncHelper = await FileSyncHelper.Create(OpenedFile);
            if (_syncHelper == null)
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


            var launchFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(Global.FileLaunchFolderName, CreationCollisionOption.OpenIfExists);
            await SimpleIoc.Default.GetInstance<IConService>().Connect(new { FilePath = Path.Combine(launchFolder.Path, Global.FileLaunchFileName) });
            (SimpleIoc.Default.GetInstance<IConService>() as Service.Sqlite.ConService).Connection.TableChanged += async (ss, ee) =>
                 await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => IsDbChanged = true);

            if (_syncHelper.IsAutoVerified)
            {
                MessageHelper.ShowPrimary("密码正确，自动打开", 5000);
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

            var result = isAnother ? await _syncHelper.SetFile(localTempFile, file => OpenedFile = file) : await _syncHelper.SetFile(localTempFile);
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
