using System.ComponentModel.DataAnnotations;

namespace Lavcode.Asp.Dtos
{
    public class CreatePasswordDto
    {
        [Required]
        public string FolderId { get; set; } = null!;

        public string? Title { get; set; }
        public string? Value { get; set; }
        public string? Remark { get; set; }

        [Required]
        public UpsertIconDto Icon { get; set; } = null!;

        [Required]
        public UpsertKeyValuePairDto[] KeyValuePairs { get; set; } = null!;
    }
}
