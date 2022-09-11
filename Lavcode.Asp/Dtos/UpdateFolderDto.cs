using System.ComponentModel.DataAnnotations;

namespace Lavcode.Asp.Dtos
{
    public class UpdateFolderDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int Order { get; set; }

        /// <summary>
        /// 空则不修改图标
        /// </summary>
        [Required]
        public UpsertIconDto? Icon { get; set; }
    }
}
