using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.View.Sync.History;
using Hubery.Tools.Uwp.Controls.Dialog;
using Hubery.Tools.Uwp.Helpers;
using System;
using Windows.Storage;

namespace Lavcode.Uwp.View.Sync
{
    public sealed partial class SyncDialog : LayoutDialog
    {
        public SyncDialog()
        {
            this.InitializeComponent();

            Loaded += Sync_Loaded;
            IsOpenChanged += OnIsOpenChanged;
        }

        private async void OnIsOpenChanged(bool isOpen)
        {
            try
            {
                if (isOpen)
                {
                    return;
                }

                var historyFolder = await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(Global.SyncTempFolderName, CreationCollisionOption.OpenIfExists);
                await historyFolder.DeleteAsync();
            }
            catch { }
        }

        private async void Sync_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (SettingHelper.Instance.SyncTaught)
            {
                return;
            }

            await PopupHelper.ShowTeachingTip(DavTitle, "网盘备份（网盘备份 1/4）", "支持Dav的网盘都可以用于备份同步，比如“坚果云”等。备份介质通过您输入的密码加密，可安全同步。开发者不会获取信息");
            await PopupHelper.ShowTeachingTip(RemoteMergeBtn, "自动合并（网盘备份 2/4）", "可以与网盘中的记录自动合并");
            await PopupHelper.ShowTeachingTip(DavToLocalBtn, "云端覆盖本地（网盘备份 3/4）", "此操作会完全将云端数据覆盖至本地，不会保留本地原始内容。需要曾经备份过才可进行此操作");
            await PopupHelper.ShowTeachingTip(LocalToDavBtn, "本地覆盖云端（网盘备份 4/4）", "此操作会完全将本地数据覆盖至云端，不会保留云端原始内容");

            await PopupHelper.ShowTeachingTip(FileTitle, "文件备份（文件备份 1/1）", "备份介质使用您输入的密码进行加密，可安全方便转移备份。操作与“网盘备份”类似");
            await PopupHelper.ShowTeachingTip(Container, "注意事项（备份与恢复 1/1）", "备份介质需要您使用密码加密。支持多窗口打开备份介质，并进行编辑和保存");

            SettingHelper.Instance.SyncTaught = true;
        }

        private async void History_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new HistoryDialog().ShowAsync();
        }

        private async void DavSetting_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new LoginDialog().ShowAsync();
        }

        private async void PasswordSetting_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new Validator().ShowAsync();
        }

        private void MergeRules_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MergeRulesTeachingTip.IsOpen = true;
        }
    }
}
