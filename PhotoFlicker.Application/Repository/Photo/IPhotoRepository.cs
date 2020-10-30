using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoFlicker.Application.Repository.Photo
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Models.Models.Photo>> Take(int amount);
        Task<IEnumerable<Models.Models.Photo>> TakeIncludeTags(int amount);
        Task<IEnumerable<Models.Models.Photo>> TakeWhereTag(int tagId, int amount);
        Task<IEnumerable<Models.Models.Photo>> TakeRandom(int amount);
        Task<IEnumerable<Models.Models.Photo>> TakeRandomWithTag(int amount, int tagId);
        Task<IEnumerable<Models.Models.Photo>> TakeWithTagLike(string pattern, int amount);
        Task<Models.Models.Photo> GetById(int id);
        Task<bool> Create(string url, IEnumerable<string> tagNames);
        Task<bool> UpdatePath(int id, string newPath);
        Task<bool> Delete(int id);
        Task<bool> AddTagToPhoto(int photoId, int tagId);
        Task<bool> RemoveTagFromPhoto(int photoId, int tagId);
        Task<bool> IsTagExist(int tagId);
        Task<string[]> FilterByExistence(IEnumerable<string> tags); 
        Task SaveChanges();
    }
}