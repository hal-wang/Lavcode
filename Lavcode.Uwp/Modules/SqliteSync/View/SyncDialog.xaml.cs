using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Guide;
using Lavcode.Uwp.Modules.SqliteSync.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.SqliteSync.View
{
    public sealed partial class SyncDialog : ContentDialog
    {
        public SyncDialog()
        {
            DataContext = VM;
            this.InitializeComponent();

            Loaded += Sync_Loaded;
        }

        public SyncViewModel VM { get; } = SimpleIoc.Default.GetInstance<SyncViewModel>();

        private async void Sync_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new GuideHandler()
            {
                SettingField = nameof(SettingHelper.SyncTaught),
                Total = 6,
                Type = "备份",
            }
            .Add(new GuideItem()
            {
                Title = "网盘备份",
                Content = "支持Dav的网盘都可以用于备份同步，比如“坚果云”等。备份介质通过您输入的密码加密，可安全同步。开发者不会获取信息",
                Target = DavTitle,
            })
            .Add(new GuideItem()
            {
                Title = "自动合并",
                Content = "可以与网盘中的记录自动合并",
                Target = RemoteMergeBtn,
            })
            .Add(new GuideItem()
            {
                Title = "云端覆盖本地",
                Content = "此操作会完全将云端数据覆盖至本地，不会保留本地原始内容。需要曾经备份过才可进行此操作",
                Target = DavToLocalBtn,
            })
            .Add(new GuideItem()
            {
                Title = "本地覆盖云端",
                Content = "此操作会完全将本地数据覆盖至云端，不会保留云端原始内容",
                Target = LocalToDavBtn,
            })
            .Add(new GuideItem()
            {
                Title = "文件备份",
                Content = "备份介质使用您输入的密码进行加密，可安全方便转移备份。操作与“网盘备份”类似",
                Target = FileTitle,
            })
            .Add(new GuideItem()
            {
                Title = "注意事项",
                Content = "备份介质需要您使用密码加密。支持多窗口打开备份介质，并进行编辑和保存",
                Target = Container,
            })
            .End()
            .RunAsync();
        }

        internal Task Queue()
        {
            throw new NotImplementedException();
        }

        private async void History_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new HistoryDialog().QueueAsync();
        }

        private async void DavSetting_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new LoginDialog().QueueAsync();
        }

        private async void PasswordSetting_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new Validator().QueueAsync();
        }

        private void MergeRules_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MergeRulesTeachingTip.IsOpen = true;
        }
    }
}
