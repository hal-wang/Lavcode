using System.ComponentModel.DataAnnotations;

namespace Lavcode.Asp.Dtos
{
    public class CreateFolderDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public UpsertIconDto Icon { get; set; } = null!;
    }
}
