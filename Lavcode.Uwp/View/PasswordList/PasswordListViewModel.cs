using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Lavcode.Uwp.Helpers.Sqlite;
using Lavcode.Uwp.Model;
using Hubery.Tools;
using Hubery.Tools.Uwp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Uwp.View.PasswordList
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
        public PasswordListViewModel()
        {
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
            using var helper = new SqliteHelper();
            foreach (var password in await helper.GetPasswords(_curFolder.Folder.Id))
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

        public void HandleAddNew()
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
            using var helper = new SqliteHelper();
            for (var i = 0; i < PasswordItems.Count; i++)
            {
                PasswordItems[i].Password.Order = i;
                await helper.UpdatePassword(PasswordItems[i].Password);
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

        private bool _isMultiSelect = false;
        public bool IsMultiSelect
        {
            get { return _isMultiSelect; }
            set { Set(ref _isMultiSelect, value); }
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
        public async void HandleDeleteItems()
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

            using SqliteHelper sqliteHelper = new SqliteHelper();
            var count = PasswordItems.Count;
            for (int i = 0, j = 0; i < count; i++)
            {
                if (SelectedItems.Contains(PasswordItems[j]))
                {
                    if (SelectedPasswordItem == PasswordItems[j])
                    {
                        SelectedPasswordItem = null;
                    }

                    await sqliteHelper.DeletePassword(PasswordItems[j].Password.Id);
                    PasswordItems.RemoveAt(j);
                }
                else
                {
                    j++;
                }
            }
        }

        public async Task DeleteSingleItem(PasswordItem passwordItem)
        {
            if (await PopupHelper.ShowDialog("即将删除 " + passwordItem.Title + "，确认操作？", "确认删除", "确认", "取消", false) != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }

            using SqliteHelper sqliteHelper = new SqliteHelper();
            await sqliteHelper.DeletePassword(passwordItem.Password.Id);

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
        public async void HandleMoveTo()
        {
            if (_curFolder == null)
            {
                return;
            }

            if (await new MoveToDialog(_curFolder.Folder, SelectedItems.Select((item) => item.Password).ToList()).ShowAsync() != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
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

                if (await new MoveToDialog(_curFolder.Folder, new List<Password>() { item.Password }).ShowAsync() != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
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
                if (data.IndexOf(Global.DragPasswordHeader) != 0)
                {
                    return;
                }

                if (_curFolder == null)
                {
                    MessageHelper.ShowWarning("请先选择一个文件夹");
                    return;
                }

                var content = data.Substring(Global.DragPasswordHeader.Length, data.Length - Global.DragPasswordHeader.Length);
                var items = JsonConvert.DeserializeObject<List<JObject>>(content);
                using var sqliteHelper = new SqliteHelper();
                foreach (JObject item in items)
                {
                    var password = item["Password"].ToObject<Password>();
                    password.FolderId = _curFolder.Folder.Id;

                    await sqliteHelper.AddPassword(password, item["Icon"].ToObject<Icon>(), item["KeyValuePairs"].ToObject<List<Model.KeyValuePair>>());
                    PasswordItems.Add(new PasswordItem(password));
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }
    }
}
