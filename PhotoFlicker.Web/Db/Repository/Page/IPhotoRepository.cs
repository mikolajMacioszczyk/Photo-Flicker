using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoFlicker.Models;

namespace PhotoFlicker.Web.Db.Repository.Page
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> Take(int amount);
        Task<IEnumerable<Photo>> TakeIncludeTags(int amount);
        Task<IEnumerable<Photo>> TakeWhereTag(int tagId, int amount);
        Task<IEnumerable<Photo>> TakeRandom(int amount);
        Task<IEnumerable<Photo>> TakeRandomWithTag(int amount, int tagId);
        Task<Photo> GetById(int id);
        Task<bool> Create(Photo created);
        Task<bool> UpdatePath(int id, string newPath);
        Task<bool> DeepDelete(int id);
        Task<bool> AddTag(int photoId, int tagId);
        Task<bool> RemoveTag(int photoId, int tagId);
        Task<bool> IsTagExist(int tagId);
        Task SaveChangesAsync();
    }
}