using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Model;
using Lavcode.Uwp.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class PasswordMoveToViewModel : ObservableObject
    {
        private readonly IFolderService _folderService;
        private readonly IPasswordService _passwordService;

        public PasswordMoveToViewModel(IFolderService folderService, IPasswordService passwordService)
        {
            _folderService = folderService;
            _passwordService = passwordService;
        }

        public ObservableCollection<FolderItem> FolderItems { get; } = new();
        public IReadOnlyList<PasswordModel> Passwords { get; private set; }
        private FolderModel _curFolder;

        private FolderItem _selectedFolder = null;
        public FolderItem SelectedFolder
        {
            get { return _selectedFolder; }
            set { SetProperty(ref _selectedFolder, value); }
        }

        public async void Init(FolderModel curFolder, IReadOnlyList<PasswordModel> passwords)
        {
            Passwords = passwords;
            _curFolder = curFolder;

            FolderItems.Clear();
            foreach (var folder in await _folderService.GetFolders())
            {
                if (folder.Id != curFolder.Id)
                {
                    FolderItems.Add(new FolderItem(folder));
                }
            }
        }

        public async Task<bool> MoveTo()
        {
            if (SelectedFolder == null)
            {
                MessageHelper.ShowWarning("请选择目标文件夹");
                return false;
            }

            await NetLoadingHelper.Invoke(async () =>
            {
                foreach (var password in Passwords)
                {
                    password.FolderId = SelectedFolder.Folder.Id;
                    await _passwordService.UpdatePassword(password);
                }
            }, "正在移动");

            MessageHelper.ShowPrimary($"移动成功！\n{_curFolder.Name}\n  ->\n{SelectedFolder.Name}", 5000);
            return true;
        }
    }
}
