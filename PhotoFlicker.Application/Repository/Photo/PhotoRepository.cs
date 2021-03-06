using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoFlicker.Application.Context;

namespace PhotoFlicker.Application.Repository.Photo
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoFlickerContext _db;

        public PhotoRepository(PhotoFlickerContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Models.Models.Photo>> Take(int amount)
        {
            if (amount < 0) { amount = 0; }
            
            return await _db.PhotoItems.Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Models.Models.Photo>> TakeIncludeTags(int amount)
        {
            if (amount < 0) { amount = 0; }

            return await _db.PhotoItems.Include(p => p.Tags).Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Models.Models.Photo>> TakeWhereTag(int tagId, int amount)
        {
            if (amount < 0) { amount = 0; }

            var tag = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null){ return new List<Models.Models.Photo>();}
            
            return await _db.PhotoItems.Include(p => p.Tags).Where(p => p.Tags.Contains(tag)).Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Models.Models.Photo>> TakeRandom(int amount)
        {
            if (amount < 0) { amount = 0; }

            return await _db.PhotoItems.OrderBy(p => Guid.NewGuid()).Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Models.Models.Photo>> TakeRandomWithTag(int amount, int tagId)
        {
            if (amount < 0) { amount = 0; }

            var tag = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null){return new List<Models.Models.Photo>();}
            
            return await _db.PhotoItems.Where(t => t.Tags.Contains(tag)).OrderBy(p => Guid.NewGuid()).Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Models.Models.Photo>> TakeWithTagLike(string pattern, int amount)
        {
            if (string.IsNullOrEmpty(pattern)) { return await _db.PhotoItems.Include(p => p.Tags).Take(amount).ToListAsync(); }
            if (amount < 0) { amount = 0; }


            return await _db.PhotoItems.Include(p => p.Tags)
                .Where(p => p.Tags.Any(t => t.Name.Contains(pattern)))
                .Take(amount)
                .ToListAsync();
        }

        public async Task<Models.Models.Photo> GetById(int id)
        {
            return await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Create(string url, IEnumerable<string> tagNames)
        {
            var photo = new Models.Models.Photo(){Path = url};
            
            
            await _db.PhotoItems.AddAsync(photo);

            var tags = new List<Models.Models.Tag>();
            foreach (var name in tagNames.Select(t => t.ToLower()))
            {
                tags.Add(await _db.TagItems.FirstAsync(t => t.Name.ToLower().Equals(name)));
            }
            photo.Tags = tags;

            return true;
        }

        public async Task<bool> UpdatePath(int id, string newPath)
        {
            if (string.IsNullOrEmpty(newPath) || newPath.Length > 50) { return false;}
            
            var fromDb = await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == id);
            if (fromDb == null)
            {
                return false;
            }

            fromDb.Path = newPath;
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var fromDb = await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == id);
            if (fromDb == null)
            {
                return false;
            }

            _db.PhotoItems.Remove(fromDb);
            return true;
        }

        public async Task<bool> AddTagToPhoto(int photoId, int tagId)
        {
            var fromDb = await _db.PhotoItems.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == photoId);
            if (fromDb == null) { return false; }

            var tag = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null) { return false; }

            if (fromDb.Tags != null && fromDb.Tags.Contains(tag)) { return true; }
            
            var tags = new List<Models.Models.Tag>(){tag};
            if (fromDb.Tags != null) { tags.AddRange(fromDb.Tags); }
            fromDb.Tags = tags;
            
            return true;
        }

        public async Task<bool> RemoveTagFromPhoto(int photoId, int tagId)
        {
            var fromDb = await _db.PhotoItems.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == photoId);
            if (fromDb == null) { return false; }

            var tag = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null) { return false; }
            
            if (fromDb.Tags == null || !fromDb.Tags.Contains(tag)) { return true; }

            var tags = new List<Models.Models.Tag>(fromDb.Tags);
            tags.Remove(tag);
            fromDb.Tags = tags;
            
            return true;
        }

        public async Task<bool> IsTagExist(int tagId)
        {
            var tag = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            return tag != null;
        }

        public async Task<string[]> FilterByExistence(IEnumerable<string> tags)
        {
            if (tags == null)
            {
                return new string[]{};
            }

            var lowerTags = tags.Where(t => !string.IsNullOrEmpty(t)).Select(t => t.ToLower());

            var filteredFromDb = await _db.TagItems.Select(t => t.Name.ToLower())
                .Where(t => lowerTags.Contains(t))
                .ToListAsync();

            return filteredFromDb.ToArray();
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}