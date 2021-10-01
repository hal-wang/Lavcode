using Lavcode.Model;

namespace Lavcode.ViewModel
{
    public class FolderItem : IconItem
    {
        public FolderItem(Folder folder, Icon icon = null)
        {
            Set(folder, icon);
        }

        public void Set(Folder folder, Icon icon = null)
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

        public Folder Folder { get; set; }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
    }
}