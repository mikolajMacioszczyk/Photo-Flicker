using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoFlicker.Web.Db.Repository.Tag
{
    public interface ITagRepository
    {
        Task<IEnumerable<Models.Tag>> Take(int amount);
        Task<IEnumerable<Models.Tag>> GetRandom(int amount);
        Task<Models.Tag> GetById(int id);
        Task<Models.Tag> GetByNameLike(string name);
        Task<bool> Create(Models.Tag created);
        Task<bool> UpdateName(int id, string newName);
        Task<bool> DeepDelete(int id);
        Task<bool> IsPhotoExist(int photoId);
        Task<bool> IsTagNameExist(string tagName);
        Task SaveChanges();
    }
}