﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Common;
using Lavcode.IService;
using Lavcode.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class PasswordListViewModel : ViewModelBase
    {
        private FolderItem _curFolder = null;

        public ObservableCollection<PasswordItem> PasswordItems { get; } = new ObservableCollection<PasswordItem>();

        private PasswordItem _selectedPasswordItem = null;
        public PasswordItem SelectedPasswordItem
        {
            get { return _selectedPasswordItem; }
            set
            {
                Set(ref _selectedPasswordItem, value);

                Messenger.Default.Send(value, "PasswordSelectedChanged");
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

            Messenger.Default.Register<FolderItem>(this, "FolderSelected", async (item) => await Init(item));
            Messenger.Default.Register<Password>(this, "PasswordAddOrEdited", (item) => PasswordAddOrEdited(item));
            Messenger.Default.Register<string>(this, "PasswordDeleted", (id) => PasswordDeleted(id));
            Messenger.Default.Register<object>(this, "OnDbRecovered", (obj) => SelectedPasswordItem = null);
        }

        ~PasswordListViewModel()
        {
            Messenger.Default.Unregister(this);
        }

        private async Task Init(FolderItem folderItem)
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

        private void PasswordAddOrEdited(Password password)
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
            Messenger.Default.Send<object>(null, "AddNewPassword");
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
            LoadingHelper.Show("正在排序");
            try
            {
                for (var i = 0; i < PasswordItems.Count; i++)
                {
                    PasswordItems[i].Password.Order = i;
                    await _passwordService.UpdatePassword(PasswordItems[i].Password);
                }
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }

        #region 多选
        private IList<PasswordItem> _selectedItems = new List<PasswordItem>();
        public IList<PasswordItem> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                Set(ref _selectedItems, value);

                RaisePropertyChanged(nameof(IsSelectAll));
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

            LoadingHelper.Show("正在删除");
            try
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
            }
            finally
            {
                LoadingHelper.Hide();
            }
        }

        public async Task DeleteSingleItem(PasswordItem passwordItem)
        {
            if (await PopupHelper.ShowDialog("即将删除 " + passwordItem.Title + "，确认操作？", "确认删除", "确认", "取消", false) != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }

            LoadingHelper.Show("正在删除");
            try
            {
                await _passwordService.DeletePassword(passwordItem.Password.Id);
            }
            finally
            {
                LoadingHelper.Hide();
            }

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

                if (!await new PasswordMoveToDialog(_curFolder.Folder, new List<Password>() { item.Password }).QueueAsync<bool>())
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

                LoadingHelper.Show("正在复制");
                try
                {
                    var content = data.Substring(CommonConstant.DragPasswordHeader.Length, data.Length - CommonConstant.DragPasswordHeader.Length);
                    var items = JsonConvert.DeserializeObject<List<JObject>>(content);
                    foreach (JObject item in items)
                    {
                        var password = item["Password"].ToObject<Password>();
                        password.FolderId = _curFolder.Folder.Id;

                        await _passwordService.AddPassword(password, item["Icon"].ToObject<Icon>(), item["KeyValuePairs"].ToObject<List<Lavcode.Model.KeyValuePair>>());
                        PasswordItems.Add(new PasswordItem(password));
                    }
                }
                finally
                {
                    LoadingHelper.Hide();
                }
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
                    passwordItem.Icon,
                    KeyValuePairs = (await _passwordService.GetKeyValuePairs(passwordItem.Password.Id)).ToArray()
                });
            }
            return items;
        }
    }
}
