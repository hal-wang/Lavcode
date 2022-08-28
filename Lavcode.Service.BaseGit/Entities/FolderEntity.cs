using Lavcode.Model;
using System;

namespace Lavcode.Service.BaseGit.Entities
{
    public class FolderEntity : IEntity
    {
        public FolderEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
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
