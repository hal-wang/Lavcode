using Lavcode.Model;
using Newtonsoft.Json;
using System;

namespace Lavcode.Service.BaseGit.Entities
{
    public class FolderEntity : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        [JsonProperty("LastEditTime")]
        public DateTime UpdatedAt { get; set; }

        public FolderModel ToModel()
        {
            return new FolderModel()
            {
                Id = Id,
                Name = Name,
                Order = Order,
                UpdatedAt = UpdatedAt,
            };
        }

        public static FolderEntity FromModel(FolderModel model)
        {
            return new FolderEntity()
            {
                Id = model.Id,
                Name = model.Name,
                Order = model.Order,
                UpdatedAt = model.UpdatedAt.DateTime,
            };
        }
    }
}
