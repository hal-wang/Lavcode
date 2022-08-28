using Lavcode.Model;
using SQLite;
using System;

namespace Lavcode.Service.Sqlite.Entities
{
    [Table("Folder")]
    public class FolderEntity
    {
        public FolderEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        [PrimaryKey]
        public string Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int Order { get; set; }

        public DateTime LastEditTime { get; set; }

        public FolderModel ToModel()
        {
            return new FolderModel()
            {
                Id = Id,
                Name = Name,
                Order = Order,
                LastEditTime = LastEditTime,
            };
        }

        public static FolderEntity FromModel(FolderModel model)
        {
            return new FolderEntity()
            {
                Id = model.Id,
                Name = model.Name,
                Order = model.Order,
                LastEditTime = model.LastEditTime,
            };
        }
    }
}
