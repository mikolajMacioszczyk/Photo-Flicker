using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhotoFlicker.Models.Dtos.Tag;

namespace PhotoFlicker.Models.Dtos.Photo
{
    public class PhotoCreateDto
    {
        [Required]
        public string Path { get; set; }
        [Required] 
        [MaxLength(50000)]
        public string Description { get; set; }
        public IEnumerable<TagCreateDto> Tags { get; set; }
    }
}