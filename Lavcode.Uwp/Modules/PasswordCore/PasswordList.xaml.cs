using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.Modules.Guide;
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
            await VM.Init(item);

            await new GuideHandler()
            {
                SettingField = nameof(SettingHelper.Instance.AddPasswordTaught),
                Total = 6,
                Type = "添加记录",
            }
            .Add(new GuideItem()
            {
                Title = "开始添加",
                Content = "点击 + 可新建一条记录，点击右侧中央“添加”按钮也行",
                Target = PasswordListCommandBar,
            })
            .Add(() =>
            {
                VM.OnAddNew();
            })
            .RunAsync(() => item != null && VM.PasswordItems.Count == 0);
        }

        private async void PasswordAddOrEdited()
        {
            await new GuideHandler()
            {
                SettingField = nameof(SettingHelper.Instance.PasswordListTaught),
                Total = 4,
                Type = "列表管理",
            }
            .Add(new GuideItem()
            {
                Title = "批量操作",
                Content = "点击此处能批量操作，可以删除或移动密码",
                Target = EditBtnPosition,
            })
            .Add(() =>
            {
                PasswordListCommandBar.SwitchMultipleManual();
            })
            .Add(new GuideItem()
            {
                Title = "完成编辑",
                Content = "再次点击即编辑完成或取消编辑",
                Target = EditBtnPosition,
            })
            .Add(() =>
            {
                PasswordListCommandBar.SwitchMultipleManual();
            })
            .Add(new GuideItem()
            {
                Title = "右键菜单",
                Content = "右键单击密码项，可单独对该密码进行删除或移动",
                Target = PasswordItemPosition,
            })
            .Add(new GuideItem()
            {
                Title = "查看详情",
                Content = "点击密码记录项即可查看或编辑",
                Target = PasswordItemPosition,
            })
            .End()
            .RunAsync();
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
