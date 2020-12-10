using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Model;
using HTools.Uwp.Helpers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View.FolderList
{
    public sealed partial class FolderList : UserControl
    {
        public FolderList()
        {
            this.InitializeComponent();
            Loaded += Folder_Loaded;

            new Action(async () => await Model.Refresh()).Invoke();
        }

        private async void Folder_Loaded(object sender, RoutedEventArgs e)
        {
            if (!SettingHelper.Instance.AddFolderTaught)
            {
                await PopupHelper.ShowTeachingTip(AddButtonPosition, "开始添加（新建文件夹 1/3）", "点击 + 开始添加文件夹，对密码进行分类管理", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.BottomRight);
                Model.HandleAddFolder();
            }
        }

        private async void EditMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is FrameworkElement element && element.DataContext is FolderItem item)
                {
                    await Model.EditFolder(item);
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
                    await Model.DeleteFolder(item);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private void AddMenu_Click(object sender, RoutedEventArgs e)
        {
            Model.HandleAddFolder();
        }
    }
}
