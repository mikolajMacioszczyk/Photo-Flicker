using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoFlicker.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public IEnumerable<Photo> Photos { get; set; }
    }
}