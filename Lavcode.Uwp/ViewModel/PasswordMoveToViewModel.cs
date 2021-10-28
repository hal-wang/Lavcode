using GalaSoft.MvvmLight;
using HTools.Uwp.Helpers;
using Lavcode.IService;
using Lavcode.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Lavcode.Uwp.ViewModel
{
    public class PasswordMoveToViewModel : ViewModelBase
    {
        private readonly IFolderService _folderService;
        private readonly IPasswordService _passwordService;

        public PasswordMoveToViewModel(IFolderService folderService, IPasswordService passwordService)
        {
            _folderService = folderService;
            _passwordService = passwordService;
        }

        public ObservableCollection<FolderItem> FolderItems { get; } = new ObservableCollection<FolderItem>();
        public IReadOnlyList<Password> Passwords { get; private set; }
        private Folder _curFolder;

        private FolderItem _selectedFolder = null;
        public FolderItem SelectedFolder
        {
            get { return _selectedFolder; }
            set { Set(ref _selectedFolder, value); }
        }

        public async void Init(Folder curFolder, IReadOnlyList<Password> passwords)
        {
            Passwords = passwords;
            _curFolder = curFolder;

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

            foreach (var password in Passwords)
            {
                password.FolderId = SelectedFolder.Folder.Id;
                await _passwordService.UpdatePassword(password);
            }

            MessageHelper.ShowPrimary($"移动成功！\n{_curFolder.Name}\n  ->\n{SelectedFolder.Name}", 5000);
            return true;
        }
    }
}
