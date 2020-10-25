using AutoMapper;
using PhotoFlicker.Models.Dtos.Photo;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Models.Profiles
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<Photo, PhotoReadDto>();
            CreateMap<PhotoCreateDto, Photo>();
        }
    }
}