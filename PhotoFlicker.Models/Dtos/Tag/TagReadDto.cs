using System.Collections.Generic;
using PhotoFlicker.Models.Dtos.Photo;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Models.Dtos.Tag
{
    public class TagReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PhotoReadDto> Photos { get; set; }
    }
}