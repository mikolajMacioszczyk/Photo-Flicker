using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PhotoFlicker.Application.Repository.Tag;
using PhotoFlicker.Models.Dtos.Tag;

namespace PhotoFlicker.Application.Service.Tag
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;
        
        public TagService(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagReadDto>> Take(int amount)
        {
            ValidateAmount(amount);
            return _mapper.Map<IEnumerable<TagReadDto>>(await _repository.Take(amount));
        }

        public async Task<IEnumerable<TagReadDto>> GetRandom(int amount)
        {
            ValidateAmount(amount);
            return _mapper.Map<IEnumerable<TagReadDto>>(await _repository.GetRandom(amount));
        }

        public async Task<IEnumerable<string>> GetRandomTagNames(int amount)
        {
            ValidateAmount(amount);
            return await _repository.GetRandomTagNames(amount);
        }

        public async Task<TagReadDto> GetById(int id)
        {
            return _mapper.Map<TagReadDto>(await _repository.GetById(id));
        }

        public async Task<TagReadDto> GetByNameLike(string name)
        {
            if (string.IsNullOrEmpty(name)) { return null;}
            return _mapper.Map<TagReadDto>(await _repository.GetByNameLike(name));
        }

        public async Task<bool> Create(TagCreateDto created)
        {
            if (created == null) { throw new ArgumentException("Created Tag cannot be null"); }

            return await _repository.Create(_mapper.Map<Models.Models.Tag>(created));
        }

        public async Task<bool> UpdateName(int id, string newName)
        {
            if (string.IsNullOrEmpty(newName)) { throw new ArgumentException("New Name cannot be empty string"); }
            
            if (await _repository.IsTagNameExist(newName)) { return false; }

            return await _repository.UpdateName(id, newName);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.DeepDelete(id);
        }

        public async Task<bool> IsPhotoExist(int photoId)
        {
            return await _repository.IsPhotoExist(photoId);
        }

        public async Task<bool> IsTagNameExist(string tagName)
        {
            if (string.IsNullOrEmpty(tagName)) { throw new ArgumentException("Cannot look by empty string"); }
            
            return await _repository.IsTagNameExist(tagName);
        }

        public async Task SaveChanges()
        {
            await _repository.SaveChanges();
        }

        private void ValidateAmount(int amount)
        {
            if (amount < 0) { throw new ArgumentException("Amount cannot be negative"); }
        }
    }
}