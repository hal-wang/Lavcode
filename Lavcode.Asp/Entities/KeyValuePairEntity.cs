using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lavcode.Asp.Entities
{
    [Table("KeyValuePair")]
    public class KeyValuePairEntity
    {
        [Key]
        public string Id { get; set; } = null!;

        public string PasswordId { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;

        public virtual PasswordEntity Password { get; set; } = null!;
    }
}
