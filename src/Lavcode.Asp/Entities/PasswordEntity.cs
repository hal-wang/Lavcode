using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lavcode.Asp.Entities
{
    [Table("Password")]
    public class PasswordEntity
    {
        public PasswordEntity()
        {
            KeyValuePairs = new HashSet<KeyValuePairEntity>();
        }

        [Key]
        public string Id { get; set; } = null!;
        public string FolderId { get; set; } = null!;

        [MaxLength(100)]
        public string? Title { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(100)]
        public string? Value { get; set; }

        [MaxLength(500)]
        public string? Remark { get; set; }

        public int Order { get; set; }

        public long UpdatedAt { get; set; }

        [ForeignKey("Id")]
        public virtual IconEntity Icon { get; set; } = null!;
        public virtual FolderEntity Folder { get; set; } = null!;
        public virtual ICollection<KeyValuePairEntity> KeyValuePairs { get; set; }
    }
}
