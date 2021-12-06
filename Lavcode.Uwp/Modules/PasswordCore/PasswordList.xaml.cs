using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class PasswordList : UserControl
    {
        public PasswordList()
        {
            DataContext = VM;
            this.InitializeComponent();

            Messenger.Default.Register<FolderItem>(this, "FolderSelected", item => FolderSelected(item));
            Messenger.Default.Register<Password>(this, "PasswordAddOrEdited", (item) => PasswordAddOrEdited());
        }

        public PasswordListViewModel VM { get; } = SimpleIoc.Default.GetInstance<PasswordListViewModel>();

        private async void FolderSelected(FolderItem item)
        {
            if (item == null || SettingHelper.Instance.AddPasswordTaught)
            {
                return;
            }

            await PopupHelper.ShowTeachingTipAsync(PasswordListCommandBar, "开始添加（添加记录 1/6）", "点击 + 可新建一条记录，点击右侧中央“添加”按钮也行");
            VM.OnAddNew();
        }

        private async void PasswordAddOrEdited()
        {
            if (SettingHelper.Instance.PasswordListTaught)
            {
                return;
            }

            await PopupHelper.ShowTeachingTipAsync(EditBtnPosition, "批量操作（列表管理 1/4）", "点击此处能批量操作，可以删除或移动密码", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.TopRight);
            PasswordListCommandBar.IsMultiSelect = true;
            await PopupHelper.ShowTeachingTipAsync(EditBtnPosition, "完成编辑（列表管理 2/4）", "再次点击即编辑完成或取消编辑", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.TopRight);
            PasswordListCommandBar.IsMultiSelect = false;
            await PopupHelper.ShowTeachingTipAsync(PasswordItemPosition, "右键菜单（列表管理 3/4）", "右键单击密码项，可单独对该密码进行删除或移动", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.RightBottom);
            await PopupHelper.ShowTeachingTipAsync(PasswordItemPosition, "查看详情（列表管理 4/4）", "点击密码记录项即可查看或编辑", Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.RightBottom);
            SettingHelper.Instance.PasswordListTaught = true;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            VM.SelectPasswordItem(e.ClickedItem as PasswordItem);
        }

        private async void ListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            try
            {
                if (args.DropResult == Windows.ApplicationModel.DataTransfer.DataPackageOperation.None)
                {
                    return;
                }

                await VM.Sort();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private void PasswordListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = new List<PasswordItem>();
            PasswordListView.SelectedItems.ToList().ForEach((item) => list.Add(item as PasswordItem));
            VM.SelectedItems = list;
        }

        #region 右键菜单
        private PasswordItem _rightClickItem = null;
        private void MenuFlyout_Opened(object sender, object e)
        {
            if (sender is MenuFlyout menuFlyout && menuFlyout.Target is ListViewItem listViewItem && listViewItem.Content is PasswordItem passwordItem)
            {
                _rightClickItem = passwordItem;
            }
            else
            {
                _rightClickItem = null;
            }
        }

        private async void Delete_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_rightClickItem == null)
            {
                return;
            }
            await VM.DeleteSingleItem(_rightClickItem);
        }

        private void MoveTo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_rightClickItem == null)
            {
                return;
            }
            VM.MoveSingleItem(_rightClickItem);
        }
        #endregion

        private async void PasswordListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var items = await VM.CreateDragItems(e.Items.Where(item => item is PasswordItem).Select(item => item as PasswordItem).ToArray());
            if (items.Count == 0)
            {
                return;
            }

            e.Data.SetText(CommonConstant.DragPasswordHeader + JsonConvert.SerializeObject(items.ToArray()));
        }

        private async void PasswordListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            if (!e.DataView.Contains(StandardDataFormats.Text))
            {
                return;
            }

            await VM.DropItems(await e.DataView.GetTextAsync());
        }

        private void PasswordListView_DragOver(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private void CB_OnSelectAll(Button sender, object args)
        {
            if (VM.IsSelectAll)
            {
                PasswordListView.SelectedItems.Clear();
            }
            else
            {
                PasswordListView.SelectAll();
            }
        }
    }
}
