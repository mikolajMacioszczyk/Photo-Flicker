using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoFlicker.Models.Dtos.Photo;

namespace PhotoFlicker.Application.Service.Photo
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoReadDto>> Take(int amount);
        Task<IEnumerable<PhotoReadDto>> TakeIncludeTags(int amount);
        Task<IEnumerable<PhotoReadDto>> TakeWhereTag(int tagId, int amount);
        Task<IEnumerable<PhotoReadDto>> TakeRandom(int amount);
        Task<IEnumerable<PhotoReadDto>> TakeRandomWithTag(int amount, int tagId);
        Task<IEnumerable<PhotoReadDto>> TakeWithTagLike(string pattern, int amount);
        Task<PhotoReadDto> GetById(int id);
        Task<bool> Create(PhotoCreateDto created);
        Task<bool> UpdatePath(int id, string newPath);
        Task<bool> Delete(int id);
        Task<bool> AddTagToPhoto(int photoId, int tagId);
        Task<bool> RemoveTagFromPhoto(int photoId, int tagId);
        Task<bool> IsTagExist(int tagId);
        Task<(bool, string[])> ValidateTasksAsPlainText(string text);
        Task SaveChanges();
    }
}