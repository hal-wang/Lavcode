using System.ComponentModel.DataAnnotations;

namespace Lavcode.Asp.Dtos
{
    public class UpdatePasswordDto
    {
        [Required]
        public string FolderId { get; set; } = null!;

        public string? Title { get; set; }
        public string? Value { get; set; }
        public string? Remark { get; set; }

        [Required]
        public int Order { get; set; }

        public UpsertIconDto? Icon { get; set; } = null!;
        public UpsertKeyValuePairDto[]? KeyValuePairs { get; set; }
    }
}
