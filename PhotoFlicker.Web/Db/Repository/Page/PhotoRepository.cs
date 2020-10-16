using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoFlicker.Models;
using PhotoFlicker.Web.Db.Context;

namespace PhotoFlicker.Web.Db.Repository.Page
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoFlickerContext _db;

        public PhotoRepository(PhotoFlickerContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Photo>> Take(int amount)
        {
            if (amount < 0) { amount = 0; }
            
            return await _db.PhotoItems.Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Photo>> TakeIncludeTags(int amount)
        {
            if (amount < 0) { amount = 0; }

            return await _db.PhotoItems.Include(p => p.Tags).Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Photo>> TakeWhereTag(int tagId, int amount)
        {
            if (amount < 0) { amount = 0; }

            var tag = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null){throw new ArgumentException("Tag z tym id nie istnieje"); }
            
            return await _db.PhotoItems.Where(p => p.Tags.Contains(tag)).Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Photo>> TakeRandom(int amount)
        {
            if (amount < 0) { amount = 0; }

            return await _db.PhotoItems.OrderBy(p => Guid.NewGuid()).Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Photo>> TakeRandomWithTag(int amount, int tagId)
        {
            if (amount < 0) { amount = 0; }

            var tag = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null){throw new ArgumentException("Tag z tym id nie istnieje"); }
            
            return await _db.PhotoItems.Where(t => t.Tags.Contains(tag)).OrderBy(p => Guid.NewGuid()).Take(amount).ToListAsync();
        }

        public async Task<Photo> GetById(int id)
        {
            return await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Create(Photo created)
        {
            if (created.Tags != null)
            {
                created.Tags = null;
            }
            await _db.AddRangeAsync(created);
            return true;
        }

        public async Task<bool> UpdatePath(int id, string newPath)
        {
            var fromDb = await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == id);
            if (fromDb == null)
            {
                return false;
            }

            fromDb.Path = newPath;
            return true;
        }

        public async Task<bool> DeepDelete(int id)
        {
            var fromDb = await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == id);
            if (fromDb == null)
            {
                return false;
            }

            _db.PhotoItems.Remove(fromDb);
            return true;
        }

        public async Task<bool> AddTag(int photoId, int tagId)
        {
            var fromDb = await _db.PhotoItems.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == photoId);
            if (fromDb == null) { return false; }

            var tag = await _db.TagItems.Include(t => t.MarkedPhotos).FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null) { return false; }

            if (fromDb.Tags != null && fromDb.Tags.Contains(tag)) { return true; }
            
            var tags = new List<Tag>(){tag};
            if (fromDb.Tags != null) { tags.AddRange(fromDb.Tags); }
            fromDb.Tags = tags;
            
            var photos = new List<Photo>(){fromDb};
            if (tag.MarkedPhotos != null) { photos.AddRange(tag.MarkedPhotos); }
            tag.MarkedPhotos = photos;

            return true;
        }

        public async Task<bool> RemoveTag(int photoId, int tagId)
        {
            var fromDb = await _db.PhotoItems.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == photoId);
            if (fromDb == null) { return false; }

            var tag = await _db.TagItems.Include(t => t.MarkedPhotos).FirstOrDefaultAsync(t => t.Id == tagId);
            if (tag == null) { return false; }
            
            if (fromDb.Tags == null || !fromDb.Tags.Contains(tag)) { return true; }

            var tags = new List<Tag>(fromDb.Tags);
            tags.Remove(tag);
            fromDb.Tags = tags;
            
            var photos = new List<Photo>(tag.MarkedPhotos);
            photos.Remove(fromDb);
            tag.MarkedPhotos = photos;

            return true;
        }

        public async Task<bool> IsTagExist(int tagId)
        {
            var tag = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            return tag != null;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}