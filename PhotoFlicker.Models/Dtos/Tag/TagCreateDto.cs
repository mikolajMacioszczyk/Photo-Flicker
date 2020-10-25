using System.ComponentModel.DataAnnotations;

namespace PhotoFlicker.Models.Dtos.Tag
{
    public class TagCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}