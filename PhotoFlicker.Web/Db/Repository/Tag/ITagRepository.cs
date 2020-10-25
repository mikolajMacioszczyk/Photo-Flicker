using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoFlicker.Web.Db.Repository.Tag
{
    public interface ITagRepository
    {
        Task<IEnumerable<Models.Models.Tag>> Take(int amount);
        Task<IEnumerable<Models.Models.Tag>> GetRandom(int amount);
        Task<Models.Models.Tag> GetById(int id);
        Task<Models.Models.Tag> GetByNameLike(string name);
        Task<bool> Create(Models.Models.Tag created);
        Task<bool> UpdateName(int id, string newName);
        Task<bool> DeepDelete(int id);
        Task<bool> IsPhotoExist(int photoId);
        Task<bool> IsTagNameExist(string tagName);
        Task SaveChanges();
    }
}