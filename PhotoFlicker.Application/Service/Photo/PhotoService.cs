using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PhotoFlicker.Application.Repository.Photo;
using PhotoFlicker.Models.Dtos.Photo;

namespace PhotoFlicker.Application.Service.Photo
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _repository;
        private readonly IMapper _mapper;

        public PhotoService(IPhotoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PhotoReadDto>> Take(int amount)
        {
            ValidateAmount(amount);

            return _mapper.Map<IEnumerable<PhotoReadDto>>(await _repository.Take(amount));
        }

        public async Task<IEnumerable<PhotoReadDto>> TakeIncludeTags(int amount)
        {
            ValidateAmount(amount);

            return _mapper.Map<IEnumerable<PhotoReadDto>>(await _repository.TakeIncludeTags(amount));
        }

        public async Task<IEnumerable<PhotoReadDto>> TakeWhereTag(int tagId, int amount)
        {
            ValidateAmount(amount);

            return _mapper.Map<IEnumerable<PhotoReadDto>>(await _repository.TakeWhereTag(tagId, amount));
        }

        public async Task<IEnumerable<PhotoReadDto>> TakeRandom(int amount)
        {
            ValidateAmount(amount);
            
            return _mapper.Map<IEnumerable<PhotoReadDto>>(await _repository.TakeRandom(amount));
        }

        public async Task<IEnumerable<PhotoReadDto>> TakeRandomWithTag(int amount, int tagId)
        {
            ValidateAmount(amount);
            
            return _mapper.Map<IEnumerable<PhotoReadDto>>(await _repository.TakeRandomWithTag(tagId, amount));
        }

        public async Task<IEnumerable<PhotoReadDto>> TakeWithTagLike(string pattern, int amount)
        {
            ValidateAmount(amount);
            if (string.IsNullOrEmpty(pattern)) { throw new ArgumentException("Pattern cannot be empty string"); }

            return _mapper.Map<IEnumerable<PhotoReadDto>>(await _repository.TakeWithTagLike(pattern, amount));
        }

        public async Task<PhotoReadDto> GetById(int id)
        {
            return _mapper.Map<PhotoReadDto>(await _repository.GetById(id));
        }

        public async Task<bool> Create(PhotoCreateDto created)
        {
            if (created == null) { throw new ArgumentException("Created item cannot be null"); }

            return await _repository.Create(_mapper.Map<Models.Models.Photo>(created));
        }

        public async Task<bool> UpdatePath(int id, string newPath)
        {
            if (string.IsNullOrEmpty(newPath)) { throw new ArgumentException("Updated path cannot be empty string"); }

            return await _repository.UpdatePath(id, newPath);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<bool> AddTagToPhoto(int photoId, int tagId)
        {
            return await _repository.AddTagToPhoto(photoId, tagId);
        }

        public async Task<bool> RemoveTagFromPhoto(int photoId, int tagId)
        {
            return await _repository.RemoveTagFromPhoto(photoId, tagId);
        }

        public async Task<bool> IsTagExist(int tagId)
        {
            return await _repository.IsTagExist(tagId);
        }

        public async Task<(bool, string[])> ValidateTasksAsPlainText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return (true, new string[]{ });
            }
            
            var splitted = text.Split('#')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .Distinct().ToList();

            RemoveFromList(splitted, await _repository.FilterByExistence(splitted));

            if (splitted.Count > 0)
            {
                return (false, splitted.ToArray());
            }
            return (true, new string[] { });
        }

        private void RemoveFromList(List<string> baseList, string[] toRemove)
        {
            foreach (var lowerCaseTagName in toRemove)
            {
                var originalTagName = baseList.FirstOrDefault(s => s.ToLower().Equals(lowerCaseTagName));
                while (originalTagName != null)
                {
                    baseList.Remove(originalTagName);
                    originalTagName = baseList.FirstOrDefault(s => s.ToLower().Equals(lowerCaseTagName));
                }
            }
        }

        public async Task SaveChanges()
        {
            await _repository.SaveChanges();
        }

        private void ValidateAmount(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative");
            }
        }
    }
}