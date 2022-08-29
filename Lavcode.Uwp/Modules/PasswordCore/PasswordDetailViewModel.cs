using Force.DeepCloner;
using HTools;
using HTools.Uwp.Controls.Message;
using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class PasswordDetailViewModel : ObservableObject
    {
        private readonly IPasswordService _passwordService;

        public PasswordDetailViewModel(IPasswordService passwordService)
        {
            _passwordService = passwordService;

            StrongReferenceMessenger.Default.Register<PasswordDetailViewModel, PasswordItem, string>(this, "PasswordSelectedChanged", async (_, item) => await InitEdit(item?.Password));
            StrongReferenceMessenger.Default.Register<PasswordDetailViewModel, object, string>(this, "AddNewPassword", (_, _) => HandleAddNew());
            StrongReferenceMessenger.Default.Register<PasswordDetailViewModel, FolderItem, string>(this, "FolderSelected", (_, item) => FolderSelected(item));

            KeyValuePairs.CollectionChanged += KeyValuePairs_CollectionChanged;

            ExitHandler.Instance.Add(OnCloseRequest, 0);
        }

        private void KeyValuePairs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs _)
        {
            OnPropertyChanged(nameof(IsKeyValuePairsVisible));
        }

        #region Folder
        private string _curFolderId = null;

        private void FolderSelected(FolderItem folderItem)
        {
            if (folderItem == null)
            {
                _curFolderId = null;
            }
            else
            {
                _curFolderId = folderItem.Folder.Id;
            }
        }
        #endregion

        #region Password属性
        public ObservableCollection<PasswordKeyValuePairItem> KeyValuePairs { get; } = new ObservableCollection<PasswordKeyValuePairItem>();

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private string _remark = string.Empty;
        public string Remark
        {
            get { return _remark; }
            set { SetProperty(ref _remark, value); }
        }
        #endregion

        private IconModel _icon = IconModel.GetDefault(StorageType.Password);
        public IconModel Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }

        private bool _readOnly = true;
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                SetProperty(ref _readOnly, value);

                OnPropertyChanged(nameof(IsKeyValuePairsVisible));
            }
        }

        private bool _isEmpty = true;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetProperty(ref _isEmpty, value); }
        }

        private bool _passwordVisible = false;
        public bool PasswordVisible
        {
            get => _passwordVisible;
            set => SetProperty(ref _passwordVisible, value);
        }

        public void OnPasswordVisibleChange()
        {
            PasswordVisible = !PasswordVisible;
        }

        private void Clean()
        {
            KeyValuePairs.Clear();
            Title = string.Empty;
            Value = string.Empty;
            Remark = string.Empty;
            Icon = IconModel.GetDefault(StorageType.Password);
        }

        public async void HandleAddNew()
        {
            if (IsEdited)
            {
                if (!await Save()) // 已经编辑，保存失败
                {
                    return;
                }
            }
            else
            {
                if (_oldPassword == null && !IsEmpty) // 当是新加且未编辑
                {
                    return;
                }
            }

            Clean();
            SetDefaultKeyValuePairs();
            IsEmpty = false;
            ReadOnly = false;
            _oldPassword = null;
            _oldIcon = null;
        }

        #region 键值对
        private readonly List<string> _defaultKeys = new()
        {
            "账号",
            "邮箱",
            "手机号",
            "网址",
            "用途",
        };

        private string GetDefaultKey()
        {
            foreach (var key in _defaultKeys)
            {
                if (KeyValuePairs.Where((item) => item.Key == key).Count() == 0)
                {
                    return key;
                }
            }
            return string.Empty;
        }

        public void HandleAddKeyValuePair()
        {
            KeyValuePairs.Add(new PasswordKeyValuePairItem(this)
            {
                Key = GetDefaultKey()
            });

            SetKeysLength();
        }

        public async Task CustomKey(PasswordKeyValuePairItem keyValuePairItem)
        {
            var dialog = new PasswordKeyValuePairCustomDialog()
            {
                Key = keyValuePairItem.Key
            };
            if (await dialog.QueueAsync() != ContentDialogResult.Primary)
            {
                return;
            }

            keyValuePairItem.Key = dialog.Key;

            SetKeysLength();
        }

        public async Task DeleteKey(PasswordKeyValuePairItem keyValuePairItem)
        {
            if (!string.IsNullOrEmpty(keyValuePairItem.Value) && await PopupHelper.ShowDialog($"确认删除{(string.IsNullOrEmpty(keyValuePairItem.Key) ? "该内容" : " " + keyValuePairItem.Key)}？", "确认删除", "确定", "点错了", false) != ContentDialogResult.Primary)
            {
                return;
            }

            if (KeyValuePairs.Contains(keyValuePairItem))
            {
                KeyValuePairs.Remove(keyValuePairItem);
            }

            SetKeysLength();
        }

        public bool IsKeyValuePairsVisible
        {
            get
            {
                if (!ReadOnly || KeyValuePairs.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void SetDefaultKeyValuePairs()
        {
            for (var i = 0; i < 3; i++)
            {
                KeyValuePairs.Add(new PasswordKeyValuePairItem(this)
                {
                    Key = GetDefaultKey()
                });
            }

            SetKeysLength();
        }

        private void SetKeyValuePairs(IList<KeyValuePairModel> keyValuePairs)
        {
            _oldKeyValuePairs.Clear();
            KeyValuePairs.Clear();

            foreach (var item in keyValuePairs)
            {
                _oldKeyValuePairs.Add(item);
                KeyValuePairs.Add(new PasswordKeyValuePairItem(item, this));
            }

            SetKeysLength();
        }

        public TextBlock CalcTextBlock { get; set; }
        private async void SetKeysLength()
        {
            var beforeWidth = CalcTextBlock.ActualWidth;
            var maxLengthKvp = KeyValuePairs.OrderByDescending((item) => string.IsNullOrEmpty(item.Key) ? 0 : item.Key.Length).FirstOrDefault();
            if (maxLengthKvp == default)
            {
                return;
            }
            CalcTextBlock.Text = maxLengthKvp.Key;

            await TaskExtend.SleepAsync();
            foreach (var keyValuePair in KeyValuePairs)
            {
                keyValuePair.KeyLength = CalcTextBlock.ActualWidth + 12;
            }
        }
        #endregion

        #region 编辑
        private PasswordModel _oldPassword = null;
        private IconModel _oldIcon = null;
        private List<KeyValuePairModel> _oldKeyValuePairs = new();

        /// <summary>
        /// 初始化详情页
        /// </summary>
        /// <param name="password">空则清空内容，否则为查看</param>
        /// <returns></returns>
        private async Task InitEdit(PasswordModel password)
        {
            if (password == null && _oldPassword == null)
            {
                return;
            }
            if (!IsEmpty && password != null && _oldPassword != null && password.Id == _oldPassword.Id)
            {
                return;
            }

            if (IsEdited && !await Save())
            {
                return;
            }

            SetPassword(password);
        }

        private void SetPassword(PasswordModel password)
        {
            if (password == null)
            {
                IsEmpty = true;
            }
            else
            {
                _oldPassword = password;
                IsEmpty = false;
                ReadOnly = true;

                Title = password.Title;
                Value = password.Value;
                Remark = password.Remark;

                SetKeyValuePairs(password.KeyValuePairs);
                Icon = _oldPassword.Icon;
                _oldIcon = Icon;
            }
        }

        public async void CancelEditCommand()
        {
            if (!IsEdited)
            {
                if (_oldPassword == null)
                {
                    IsEmpty = true;
                }
                else
                {
                    ReadOnly = true;
                }
            }
            else
            {
                if (await PopupHelper.ShowDialog("已编辑内容但未保存，确认放弃？", "未保存", "确定", "取消") != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
                {
                    return;
                }

                if (_oldPassword == null)
                {
                    IsEmpty = true;
                }
                else
                {
                    SetPassword(_oldPassword);
                    ReadOnly = true;
                }
            }

        }

        /// <summary>
        /// 判断有没有编辑内容。
        /// 未编辑：
        /// 1、添加但未输入内容
        /// 2、编辑但未修改内容
        /// 3、未显示内容或内容只读
        /// </summary>
        /// <returns></returns>
        public bool IsEdited
        {
            get
            {
                if (IsEmpty || ReadOnly)
                {
                    return false;
                }

                if (_oldPassword == null) //添加，添加时不考虑仅编辑图标的情况
                {
                    if (!string.IsNullOrEmpty(Title))
                    {
                        return true;
                    }
                    if (!string.IsNullOrEmpty(Remark))
                    {
                        return true;
                    }
                    if (!string.IsNullOrEmpty(Value))
                    {
                        return true;
                    }
                    if (KeyValuePairs.Count > 0 && KeyValuePairs.Where((item) => !string.IsNullOrEmpty(item.Value)).Count() > 0)
                    {
                        return true;
                    }
                }
                else //编辑
                {
                    if (_oldPassword.Title != Title || _oldPassword.Value != Value || _oldPassword.Remark != Remark)
                    {
                        return true;
                    }

                    var kvps = KeyValuePairs.Where((item) => !string.IsNullOrEmpty(item.Value)).ToList();
                    if (_oldKeyValuePairs.Count != kvps.Count)
                    {
                        return true;
                    }
                    for (int i = 0; i < _oldKeyValuePairs.Count; i++)
                    {
                        if (_oldKeyValuePairs[i].Key != kvps[i].Key || _oldKeyValuePairs[i].Value != kvps[i].Value)
                        {
                            return true;
                        }
                    }

                    if ((_oldPassword == null && !Icon.IsDefault) || (_oldPassword != null && !Icon.Equals(_oldIcon)))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void EditCommand()
        {
            ReadOnly = false;
        }
        #endregion

        #region 保存
        public async void HandleSave()
        {
            try
            {
                await Save();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        private async Task<bool> Save()
        {
            if (!IsEdited)
            {
                if (_oldPassword == null) //添加，但没内容
                {
                    MessageHelper.ShowInfo("无内容，未保存");
                    return false;
                }
                else //编辑未修改
                {
                    MessageHelper.ShowInfo("未修改，未保存");
                    ReadOnly = true;
                    return true;
                }
            }

            if (_curFolderId == null && _oldPassword == null)
            {
                MessageHelper.ShowWarning("请先选择一个文件夹");
                return false;
            }

            var newPassword = _oldPassword == null
                ? new PasswordModel()
                {
                    FolderId = _curFolderId
                }
                : _oldPassword.DeepClone();
            newPassword.Title = Title;
            newPassword.Value = Value;
            newPassword.Remark = Remark;
            newPassword.Icon = Icon;
            newPassword.KeyValuePairs = CurKeyValuePairs;

            var execResult = await NetLoadingHelper.Invoke<bool>(async () =>
            {
                if (_oldPassword == null)
                {
                    await _passwordService.AddPassword(newPassword);
                }
                else
                {
                    await _passwordService.UpdatePassword(newPassword, false, false);
                }
                return true;
            }, "正在保存");
            if (!execResult) return false;

            _oldKeyValuePairs = CurKeyValuePairs;
            _oldPassword = newPassword;
            _oldIcon = Icon;

            StrongReferenceMessenger.Default.Send(newPassword, "PasswordAddOrEdited");
            MessageHelper.ShowPrimary("保存完成");

            ReadOnly = true;
            return true;
        }

        /// <summary>
        /// 当前编辑的键值对
        /// </summary>
        private List<KeyValuePairModel> CurKeyValuePairs
        {
            get
            {
                List<KeyValuePairModel> result = new();
                foreach (var kvp in KeyValuePairs.Where((item) => !string.IsNullOrEmpty(item.Value)))
                {
                    result.Add(new KeyValuePairModel()
                    {
                        Key = kvp.Key,
                        Value = kvp.Value,
                    });
                }
                return result;
            }
        }

        public async Task<bool> OnCloseRequest()
        {
            if (!IsEdited)
            {
                return true;
            }

            var cdr = await PopupHelper.ShowDialog("当前内容已修改但未保存，是否保存？", "编辑未保存", "保存并退出", "不保存退出", null, true, "点错了");
            if (cdr == ContentDialogResult.Secondary || (cdr == ContentDialogResult.Primary && await Save()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Delete
        public async void DeleteCommand()
        {
            if (_oldPassword == null) //正常情况不会满足这个条件
            {
                IsEmpty = true;
                return;
            }

            if (await PopupHelper.ShowDialog($"确认删除{(string.IsNullOrEmpty(Title) ? "该记录" : $" {Title} ")}？该操作不可恢复！", "确认删除", "确认", "取消") != ContentDialogResult.Primary)
            {
                return;
            }


            await NetLoadingHelper.Invoke(() => _passwordService.DeletePassword(_oldPassword.Id), "正在删除");
            StrongReferenceMessenger.Default.Send(_oldPassword.Id, "PasswordDeleted");
            IsEmpty = true;
        }
        #endregion

        #region 复制
        private void CopyStr(string key, string value, Button button)
        {
            if (string.IsNullOrEmpty(value))
            {
                MessageHelper.ShowSticky(button, "无内容", MessageType.Warning);
                return;
            }

            var dataPackage = new DataPackage();
            dataPackage.SetText(value);
            Clipboard.SetContent(dataPackage);

            MessageHelper.ShowSticky(button, $"已复制 {key}\n受限于UWP，请在关闭软件前粘贴", MessageType.Primary);
        }

        public void CopyPswd(object sender, RoutedEventArgs _)
        {
            CopyStr("密码", Value, sender as Button);
        }

        public void CopyRemark(object sender, RoutedEventArgs _)
        {
            CopyStr("备注", Remark, sender as Button);
        }

        public void CopyKeyValue(PasswordKeyValuePairItem item, Button button)
        {
            CopyStr(item.Key, item.Value, button);
        }
        #endregion
    }
}