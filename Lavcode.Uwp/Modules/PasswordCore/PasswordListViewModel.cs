using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class PasswordListViewModel : ObservableObject
    {
        private FolderItem _curFolder = null;

        public ObservableCollection<PasswordItem> PasswordItems { get; } = new ObservableCollection<PasswordItem>();

        private PasswordItem _selectedPasswordItem = null;
        public PasswordItem SelectedPasswordItem
        {
            get { return _selectedPasswordItem; }
            set
            {
                SetProperty(ref _selectedPasswordItem, value);

                StrongReferenceMessenger.Default.Send(value, "PasswordSelectedChanged");
            }
        }

        public void SelectPasswordItem(PasswordItem passwordItem)
        {
            SelectedPasswordItem = passwordItem;
        }

        #region Init
        private readonly IPasswordService _passwordService;

        public PasswordListViewModel(IPasswordService passwordService)
        {
            _passwordService = passwordService;

        }

        public void RegisterMsg()
        {
            StrongReferenceMessenger.Default.Register<PasswordListViewModel, PasswordModel, string>(this, "PasswordAddOrEdited", (_, item) => PasswordAddOrEdited(item));
            StrongReferenceMessenger.Default.Register<PasswordListViewModel, string, string>(this, "PasswordDeleted", (_, id) => PasswordDeleted(id));
            StrongReferenceMessenger.Default.Register<PasswordListViewModel, object, string>(this, "OnDbRecovered", (_, _) => SelectedPasswordItem = null);
        }

        public void UnregisterMsg()
        {
            StrongReferenceMessenger.Default.UnregisterAll(this);
        }

        public async Task Init(FolderItem folderItem)
        {
            if (folderItem == null && _curFolder == null)
            {
                return;
            }

            _curFolder = folderItem;
            if (SelectedPasswordItem != null)
            {
                SelectedPasswordItem = null;
            }
            PasswordItems.Clear();
            if (folderItem == null)
            {
                return;
            }

            await foreach (var password in GetPasswordItems())
            {
                PasswordItems.Add(password);
            }
        }

        private async IAsyncEnumerable<PasswordItem> GetPasswordItems()
        {
            foreach (var password in await _passwordService.GetPasswords(_curFolder.Folder.Id))
            {
                PasswordItem passwordItem = null;
                await TaskExtend.Run(() =>
                {
                    passwordItem = new PasswordItem(password);
                });
                yield return passwordItem;
            }
        }

        #endregion

        private void PasswordAddOrEdited(PasswordModel password)
        {
            if (_curFolder == null || _curFolder.Folder.Id != password.FolderId)
            {
                return;
            }

            var queryResult = PasswordItems.Where((item) => item.Password.Id == password.Id);
            if (queryResult.Count() == 0)
            {
                PasswordItems.Add(new PasswordItem(password));
            }
            else
            {
                var existItem = queryResult.First();
                existItem.Set(password);
            }
            SelectedPasswordItem = PasswordItems.Where((item) => item.Password == password).First();
        }

        public void OnAddNew()
        {
            StrongReferenceMessenger.Default.Send<object, string>(null, "AddNewPassword");
        }

        private void PasswordDeleted(string passwordId)
        {
            try
            {
                foreach (var item in PasswordItems)
                {
                    if (item.Password.Id == passwordId)
                    {
                        PasswordItems.Remove(item);
                        break;
                    }
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
                for (var i = 0; i < PasswordItems.Count; i++)
                {
                    PasswordItems[i].Password.Order = i;
                    await _passwordService.UpdatePassword(PasswordItems[i].Password, true);
                }
            }, "正在排序");
        }

        #region 多选
        private IList<PasswordItem> _selectedItems = new List<PasswordItem>();
        public IList<PasswordItem> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                SetProperty(ref _selectedItems, value);

                OnPropertyChanged(nameof(IsSelectAll));
            }
        }

        public bool IsSelectAll
        {
            get
            {
                return SelectedItems != null && SelectedItems.Count > 0 && SelectedItems.Count == PasswordItems.Count;
            }
        }
        #endregion


        #region 删除
        public async void OnDeleteItems()
        {
            if (SelectedItems.Count == 0)
            {
                MessageHelper.ShowInfo("未选择");
                return;
            }
            if (await PopupHelper.ShowDialog("即将删除 " + SelectedItems.Count + " 项，确认操作？", "确认删除", "确认", "取消", false) != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }

            await NetLoadingHelper.Invoke(async () =>
            {
                var count = PasswordItems.Count;
                for (int i = 0, j = 0; i < count; i++)
                {
                    if (SelectedItems.Contains(PasswordItems[j]))
                    {
                        if (SelectedPasswordItem == PasswordItems[j])
                        {
                            SelectedPasswordItem = null;
                        }

                        await _passwordService.DeletePassword(PasswordItems[j].Password.Id);
                        PasswordItems.RemoveAt(j);
                    }
                    else
                    {
                        j++;
                    }
                }
            }, "正在删除");
        }

        public async Task DeleteSingleItem(PasswordItem passwordItem)
        {
            if (await PopupHelper.ShowDialog("即将删除 " + passwordItem.Title + "，确认操作？", "确认删除", "确认", "取消", false) != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }

            await NetLoadingHelper.Invoke(() => _passwordService.DeletePassword(passwordItem.Password.Id), "正在删除");

            if (PasswordItems.Contains(passwordItem))
            {
                PasswordItems.Remove(passwordItem);
                if (SelectedPasswordItem == passwordItem)
                {
                    SelectedPasswordItem = null;
                }
            }
            MessageHelper.ShowPrimary("删除完成");
        }
        #endregion


        #region 移动
        public async void OnMoveTo()
        {
            if (_curFolder == null)
            {
                return;
            }

            if (!await new PasswordMoveToDialog(_curFolder.Folder, SelectedItems.Select((item) => item.Password).ToList()).QueueAsync<bool>())
            {
                return;
            }

            foreach (var item in this.SelectedItems)
            {
                PasswordItems.Remove(item);
                if (SelectedPasswordItem == item)
                {
                    SelectedPasswordItem = null;
                }
            }
        }

        public async void MoveSingleItem(PasswordItem item)
        {
            try
            {
                if (_curFolder == null)
                {
                    return;
                }

                if (!await new PasswordMoveToDialog(_curFolder.Folder, new List<PasswordModel>() { item.Password }).QueueAsync<bool>())
                {
                    return;
                }

                PasswordItems.Remove(item);
                if (SelectedPasswordItem == item)
                {
                    SelectedPasswordItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
        #endregion

        public async Task DropItems(string data)
        {
            try
            {
                if (data.IndexOf(CommonConstant.DragPasswordHeader) != 0)
                {
                    return;
                }

                if (_curFolder == null)
                {
                    MessageHelper.ShowWarning("请先选择一个文件夹");
                    return;
                }

                await NetLoadingHelper.Invoke(async () =>
                {
                    var content = data.Substring(CommonConstant.DragPasswordHeader.Length, data.Length - CommonConstant.DragPasswordHeader.Length);
                    var items = JsonConvert.DeserializeObject<List<JObject>>(content);
                    foreach (JObject item in items)
                    {
                        var password = item["Password"].ToObject<PasswordModel>();
                        password.FolderId = _curFolder.Folder.Id;

                        await _passwordService.AddPassword(password, item["KeyValuePairs"].ToObject<List<KeyValuePairModel>>());
                        PasswordItems.Add(new PasswordItem(password));
                    }
                }, "正在复制");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        public async Task<List<object>> CreateDragItems(PasswordItem[] passwordItems)
        {
            var items = new List<object>();
            foreach (var passwordItem in passwordItems)
            {
                items.Add(new
                {
                    passwordItem.Password,
                    KeyValuePairs = (await _passwordService.GetKeyValuePairs(passwordItem.Password.Id)).ToArray()
                });
            }
            return items;
        }
    }
}
