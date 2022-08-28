using Lavcode.Model;
using System;

namespace Lavcode.Service.BaseGit.Entities
{
    public class IconEntity : IEntity
    {
        public IconEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public IconType IconType { get; set; }
        public string Value { get; set; }

        public IconModel ToModel()
        {
            return new IconModel()
            {
                Id = Id,
                IconType = IconType,
                Value = Value
            };
        }

        public static IconEntity FromModel(IconModel model)
        {
            return new IconEntity()
            {
                Id = model.Id,
                IconType = model.IconType,
                Value = model.Value
            };
        }
    }
}