using System.ComponentModel.DataAnnotations;

namespace Lavcode.Asp.Dtos
{
    public class UpsertKeyValuePairDto
    {
        [Required]
        public string Key { get; set; } = null!;
        [Required]
        public string Value { get; set; } = null!;
    }
}
