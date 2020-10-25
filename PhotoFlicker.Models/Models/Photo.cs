using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoFlicker.Models.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Path { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}