using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoFlicker.Models.Dtos.Tag;

namespace PhotoFlicker.Application.Service.Tag
{
    public interface ITagService
    {
        Task<IEnumerable<TagReadDto>> Take(int amount);
        Task<IEnumerable<TagReadDto>> GetRandom(int amount);
        Task<IEnumerable<string>> GetRandomTagNames(int amount);
        Task<TagReadDto> GetById(int id);
        Task<TagReadDto> GetByNameLike(string name);

        Task<bool> Create(TagCreateDto created);
        Task<bool> UpdateName(int id, string newName);
        Task<bool> Delete(int id);

        Task<bool> IsPhotoExist(int photoId);
        Task<bool> IsTagNameExist(string tagName);
        Task SaveChanges();
    }
}