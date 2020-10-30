using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoFlicker.Application.Context;

namespace PhotoFlicker.Application.Repository.Tag
{
    public class TagRepository : ITagRepository
    {
        private static Random _random = new Random();
        private readonly PhotoFlickerContext _db;

        public TagRepository(PhotoFlickerContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Models.Models.Tag>> Take(int amount)
        {
            if (amount < 0) { amount = 0; }
            
            return await _db.TagItems.Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Models.Models.Tag>> GetRandom(int amount)
        {
            if (amount < 0) { amount = 0;}

            if (amount > await _db.TagItems.CountAsync()) { return await _db.TagItems.ToListAsync(); }

            var wholeList = await _db.TagItems.ToListAsync();

            return await GenerateRandomList(amount, wholeList);
        }

        public async Task<IEnumerable<string>> GetRandomTagNames(int amount)
        {
            if (amount < 0) { amount = 0;}

            if (amount > await _db.TagItems.CountAsync()) { return await _db.TagItems.Select(t => t.Name).ToListAsync(); }
            
            var wholeList = await _db.TagItems.Select(t => t.Name).ToListAsync();

            return await GenerateRandomList<string>(amount, wholeList);
        }

        private async Task<List<T>> GenerateRandomList<T>(int amount, List<T> fromList)
        {
            List<int> indexes = await GetRandomSortedIndexes(amount, fromList.Count);

            var output = new List<T>();
            foreach (var index in indexes)
            {
                output.Add(fromList[index]);
            }
            return output;
        }

        private async Task<List<int>> GetRandomSortedIndexes(int amount, int maxIdx)
        {
            List<int> output = new List<int>(amount);
            for (int i = 0; i < amount; i++)
            {
                var idx = _random.Next(maxIdx);
                if (output.Contains(idx))
                {
                    i--;
                }
                else
                {
                    output.Add(idx);
                }
            }
            return output.OrderBy(i => i).ToList();
        }

        public async Task<Models.Models.Tag> GetById(int id)
        {
            return await _db.TagItems.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Models.Models.Tag> GetByNameLike(string name)
        {
            if (string.IsNullOrEmpty(name)) { return null;}
            name = name.ToLower();
            return await _db.TagItems.FirstOrDefaultAsync(t => t.Name.ToLower().Contains(name));
        }

        public async Task<bool> Create(Models.Models.Tag created)
        {
            await _db.TagItems.AddAsync(created);
            return true;
        }

        public async Task<bool> UpdateName(int id, string newName)
        {
            var fromDb = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == id);
            if (fromDb == null) {return false;}

            fromDb.Name = newName;
            return true;
        }

        public async Task<bool> DeepDelete(int id)
        {
            var fromDb = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == id);
            if (fromDb == null) {return false;}

            _db.Remove(fromDb);
            return true;
        }

        public async Task<bool> IsPhotoExist(int photoId)
        {
            var fromDb = await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == photoId);
            return (fromDb != null);
        }

        public async Task<bool> IsTagNameExist(string tagName) {
            var lowerTagName = tagName.ToLower();
            var fromDb = await _db.TagItems.FirstOrDefaultAsync(t => t.Name.ToLower().Equals(lowerTagName));

            return fromDb == null;
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}