using Lavcode.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lavcode.Asp.Entities
{
    [Table("Icon")]
    public class IconEntity
    {
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType IconType { get; set; }

        /// <summary>
        /// 字体：1字符+字体名称
        /// 图片：Base64字符串
        /// </summary>
        [MaxLength(1024 * 1024 * 10)]
        public string Value { get; set; } = null!;
    }
}
