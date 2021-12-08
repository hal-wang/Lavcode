using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Guide;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class FolderList : UserControl
    {
        public FolderList()
        {
            DataContext = VM;
            this.InitializeComponent();
            Loaded += Folder_Loaded;
        }

        public FolderListViewModel VM { get; } = SimpleIoc.Default.GetInstance<FolderListViewModel>();

        private async void Folder_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Folder_Loaded;
            await VM.Refresh();

            await new GuideHandler()
            {
                SettingField = nameof(SettingHelper.AddFolderTaught),
                Total = 3,
                Type = "新建文件夹",
            }
            .Add(new GuideItem()
            {
                Title = "开始添加",
                Content = "点击 + 开始添加文件夹，对密码进行分类管理",
                Placement = Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.BottomRight,
                Target = AddButtonPosition,
            })
            .Add(() =>
            {
                VM.HandleAddFolder();
            })
            .RunAsync(() => VM.FolderItems.Count == 0);
        }

        private async void EditMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is FrameworkElement element && element.DataContext is FolderItem item)
                {
                    await VM.EditFolder(item);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private async void DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is FrameworkElement element && element.DataContext is FolderItem item)
                {
                    await VM.DeleteFolder(item);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private void AddMenu_Click(object sender, RoutedEventArgs e)
        {
            VM.HandleAddFolder();
        }
    }
}
