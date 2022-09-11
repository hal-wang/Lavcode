using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lavcode.Asp.Entities
{
    [Table("Folder")]
    public class FolderEntity
    {
        public FolderEntity()
        {
            Passwords = new HashSet<PasswordEntity>();
        }

        [Key]
        public string Id { get; set; } = null!;

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int Order { get; set; }

        public long UpdatedAt { get; set; }

        [ForeignKey("Id")]
        public virtual IconEntity Icon { get; set; } = null!;
        public virtual ICollection<PasswordEntity> Passwords { get; set; }
    }
}
