using System.Collections.Generic;
using PhotoFlicker.Models.Dtos.Tag;

namespace PhotoFlicker.Models.Dtos.Photo
{
    public class PhotoReadDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public IEnumerable<TagReadDto> Tags { get; set; }
    }
}