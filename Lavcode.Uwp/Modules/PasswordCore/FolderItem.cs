using Lavcode.Model;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class FolderItem : IconItem
    {
        public FolderItem(FolderModel folder, IconModel icon = null)
        {
            Set(folder, icon);
        }

        public void Set(FolderModel folder, IconModel icon = null)
        {
            Folder = folder;
            Name = folder.Name;

            if (icon == null)
            {
                //后台设置图标
                SetIcon(folder.Id);
            }
            else
            {
                Icon = icon;
            }
        }

        public FolderModel Folder { get; set; }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}