using GalaSoft.MvvmLight;
using Hubery.Lavcode.Uwp.Helpers.Sqlite;
using Hubery.Lavcode.Uwp.Model;
using Hubery.Lavcode.Uwp.View.FolderList;
using Hubery.Tools.Uwp.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.View.PasswordList
{
    class MoveToViewModel : ViewModelBase
    {
        public ObservableCollection<FolderItem> FolderItems { get; } = new ObservableCollection<FolderItem>();
        public IReadOnlyList<Password> Passwords { get; private set; }
        private Folder _curFolder;

        private FolderItem _selectedFolder = null;
        public FolderItem SelectedFolder
        {
            get { return _selectedFolder; }
            set { Set(ref _selectedFolder, value); }
        }

        public async void Init(Model.Folder curFolder, IReadOnlyList<Model.Password> passwords)
        {
            Passwords = passwords;
            _curFolder = curFolder;

            await foreach (var folder in FolderHelper.GetFolderItems())
            {
                if (folder.Folder.Id != curFolder.Id)
                {
                    FolderItems.Add(folder);
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

            using SqliteHelper sqliteHelper = new SqliteHelper();
            foreach (var password in Passwords)
            {
                password.FolderId = SelectedFolder.Folder.Id;
                await sqliteHelper.UpdatePassword(password);
            }

            MessageHelper.ShowPrimary($"移动成功！\n{_curFolder.Name}\n  ->\n{SelectedFolder.Name}", 5000);
            return true;
        }
    }
}
