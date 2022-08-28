using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class FolderListViewModel : ObservableObject
    {
        public ObservableCollection<FolderItem> FolderItems { get; } = new ObservableCollection<FolderItem>();
        private FolderItem _selectedItem = null;
        public FolderItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_initing || _selectedItem == value || IsDragging)
                {
                    OnPropertyChanged();
                    return;
                }

                SetProperty(ref _selectedItem, value);
                SettingHelper.Instance.SelectedFolderId = value?.Folder?.Id;
                StrongReferenceMessenger.Default.Send(value, "FolderSelected");
            }
        }

        private bool _initing = false;

        public void RegisterMsg()
        {
            StrongReferenceMessenger.Default.Register<FolderListViewModel, object, string>(this, "OnDbRecovered", async (_, _) => await Refresh());
        }

        public void UnregisterMsg()
        {
            StrongReferenceMessenger.Default.UnregisterAll(this);
        }


        #region Init
        private readonly IFolderService _folderService;

        public FolderListViewModel(IFolderService folderService)
        {
            _folderService = folderService;
        }

        public async Task Refresh()
        {
            try
            {
                _initing = true;
                FolderItems.Clear();
                foreach (var folder in await _folderService.GetFolders())
                {
                    FolderItems.Add(new FolderItem(folder));
                }
                _initing = false;

                var beforeFolder = SelectedItem;
                SelectedItem
                    = FolderItems.FirstOrDefault(item => item.Folder.Id == SettingHelper.Instance.SelectedFolderId)
                    ?? FolderItems.FirstOrDefault();
                if (SelectedItem == beforeFolder) // 选中的文件夹没有变化
                {
                    OnPropertyChanged(nameof(SelectedItem));
                    StrongReferenceMessenger.Default.Send(SelectedItem, "FolderSelected");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
        #endregion

        public async void HandleAddFolder()
        {
            var editFolder = new FolderEditDialog();
            if (!await editFolder.QueueAsync<bool>()) return;

            var newFolderItem = new FolderItem(editFolder.Folder, editFolder.Icon);
            FolderItems.Add(newFolderItem);
            SelectedItem = newFolderItem;
        }

        public async Task DeleteFolder(FolderItem folderItem)
        {
            if (await PopupHelper.ShowDialog($"确认删除{folderItem.Name}及其中所有密码项？该操作不可恢复！", "确认删除", "确定", "取消", false) != ContentDialogResult.Primary)
            {
                return;
            }

            try
            {
                if (folderItem == null || !FolderItems.Contains(folderItem)) //正常情况不满足
                {
                    return;
                }

                await NetLoadingHelper.Invoke(() => _folderService.DeleteFolder(folderItem.Folder.Id), "正在删除");

                FolderItems.Remove(folderItem);

                if (FolderItems.Count == 0) // 删完了
                {
                    StrongReferenceMessenger.Default.Send<FolderItem, string>(null, "FolderSelected");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        public async Task EditFolder(FolderItem folderItem)
        {
            var editFolder = new FolderEditDialog(folderItem.Folder);
            if (await editFolder.QueueAsync<bool>())
            {
                folderItem.Set(editFolder.Folder, editFolder.Icon);
            }
        }

        #region 排序
        public bool IsDragging = false;

        public async void DragCompleted(TabView sender, TabViewTabDragCompletedEventArgs args)
        {
            IsDragging = false;
            try
            {
                if (args.DropResult == Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move)
                {
                    await Sort();
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }

        }

        public async Task Sort()
        {
            await NetLoadingHelper.Invoke(async () =>
            {
                for (var i = 0; i < FolderItems.Count; i++)
                {
                    FolderItems[i].Folder.Order = i;
                    await _folderService.UpdateFolder(FolderItems[i].Folder, true);
                }
            }, "正在排序");
        }

        public void DragStarting()
        {
            IsDragging = true;
        }
        #endregion
    }
}
