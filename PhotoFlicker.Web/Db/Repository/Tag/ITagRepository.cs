using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoFlicker.Web.Db.Repository.Tag
{
    public interface ITagRepository
    {
        Task<IEnumerable<Models.Tag>> Take(int amount);
        Task<IEnumerable<Models.Tag>> TakeIncludeMarkedPhotos(int amount);
        Task<IEnumerable<Models.Tag>> TakeWherePhoto(int photoId, int amount);
        Task<Models.Tag> GetRandom();
        Task<Models.Tag> GetById(int id);
        Task<Models.Tag> GetByName(string name);
        Task<bool> Create(Models.Tag created);
        Task<bool> UpdateName(int id, string newName);
        Task<bool> AddPhotoAsMarked(int photoId, int tagId);
        Task<bool> RemovePhotoFromMarked(int photoId, int tagId);
        Task<bool> DeepDelete(int id);
        Task<bool> IsPhotoExist(int photoId);
        Task SaveChanges();
    }
}