using AutoMapper;
using PhotoFlicker.Models.Dtos.Tag;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Models.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagReadDto>();
            CreateMap<TagCreateDto, Tag>();
        }
    }
}