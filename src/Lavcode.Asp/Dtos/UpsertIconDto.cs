using Lavcode.Model;
using System.ComponentModel.DataAnnotations;

namespace Lavcode.Asp.Dtos
{
    public class UpsertIconDto
    {
        [Required]
        public IconType IconType { get; set; }

        [Required]
        public string Value { get; set; } = null!;
    }
}
