using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
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

            new Action(async () => await VM.Refresh()).Invoke();
        }

        public FolderListViewModel VM { get; } = SimpleIoc.Default.GetInstance<FolderListViewModel>();

        private async void Folder_Loaded(object sender, RoutedEventArgs e)
        {
            if (!SettingHelper.Instance.AddFolderTaught)
            {
                await PopupHelper.ShowTeachingTipAsync(AddButtonPosition, "开始添加（新建文件夹 1/3）", "点击 + 开始添加文件夹，对密码进行分类管理", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.BottomRight);
                VM.HandleAddFolder();
            }
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
