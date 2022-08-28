using Lavcode.Model;
using SQLite;
using System;

namespace Lavcode.Service.Sqlite.Entities
{
    [Table("Icon")]
    public class IconEntity
    {
        public IconEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType IconType { get; set; }

        /// <summary>
        /// 字体：1字符+字体名称
        /// 图片：Base64字符串
        /// </summary>
        [MaxLength(1024 * 1024 * 10)]
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