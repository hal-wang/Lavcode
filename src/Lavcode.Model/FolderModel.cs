using System;

namespace Lavcode.Model
{
    public class FolderModel
    {
        public FolderModel()
        {
            Icon = IconModel.GetDefault(StorageType.Folder);
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public IconModel Icon { get; set; }
    }
}