using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoFlicker.Models;
using PhotoFlicker.Web.Db.Context;

namespace PhotoFlicker.Web.Db.Repository.Tag
{
    public class TagRepository : ITagRepository
    {
        private static Random _random = new Random();
        private readonly PhotoFlickerContext _db;

        public TagRepository(PhotoFlickerContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Models.Tag>> Take(int amount)
        {
            if (amount < 0) { amount = 0; }
            
            return await _db.TagItems.Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Models.Tag>> TakeIncludeMarkedPhotos(int amount)
        {
            if (amount < 0) { amount = 0; }

            return await _db.TagItems.Include(p => p.MarkedPhotos).Take(amount).ToListAsync();
        }

        public async Task<IEnumerable<Models.Tag>> TakeWherePhoto(int photoId, int amount)
        {
            var photo = await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == photoId);
            if (photo == null) {throw new ArgumentException("Photo o tym id nie istnieje w bazie danych");}
            if (amount < 0) { amount = 0; }

            return await _db.TagItems.Where(p => p.MarkedPhotos.Contains(photo)).Take(amount).ToListAsync();
        }

        public async Task<Models.Tag> GetRandom()
        {
            var idx = _random.Next(1, await _db.TagItems.CountAsync());
            return await _db.TagItems.Skip(idx).FirstOrDefaultAsync();
        }

        public async Task<Models.Tag> GetById(int id)
        {
            return await _db.TagItems.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Models.Tag> GetByName(string name)
        {
            name = name.ToLower();
            return await _db.TagItems.FirstOrDefaultAsync(t => t.Name.ToLower() == name);
        }

        public async Task<bool> Create(Models.Tag created)
        {
            if (created.MarkedPhotos != null)
            {
                created.MarkedPhotos = null;
            }

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

        public async Task<bool> AddPhotoAsMarked(int photoId, int tagId)
        {
            var tagFromDb = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tagFromDb == null) {return false;}
            
            var photoFromDb = await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == photoId);
            if (photoFromDb == null) {return false;}

            if (tagFromDb.MarkedPhotos != null && tagFromDb.MarkedPhotos.Contains(photoFromDb)) { return true; }
            
            var photos = new List<Photo>(){photoFromDb};
            if (tagFromDb.MarkedPhotos != null) { photos.AddRange(tagFromDb.MarkedPhotos); }
            tagFromDb.MarkedPhotos = photos;
            
            var tags = new List<Models.Tag>(){tagFromDb};
            if (photoFromDb.Tags != null) { tags.AddRange(photoFromDb.Tags); }
            photoFromDb.Tags = tags;
            
            return true;
        }

        public async Task<bool> RemovePhotoFromMarked(int photoId, int tagId)
        {
            var tagFromDb = await _db.TagItems.FirstOrDefaultAsync(t => t.Id == tagId);
            if (tagFromDb == null) {return false;}
            
            var photoFromDb = await _db.PhotoItems.FirstOrDefaultAsync(p => p.Id == photoId);
            if (photoFromDb == null) {return false;}

            if (tagFromDb.MarkedPhotos == null || !tagFromDb.MarkedPhotos.Contains(photoFromDb)) { return true; }
            
            var photos = new List<Photo>(tagFromDb.MarkedPhotos);
            photos.Remove(photoFromDb);
            tagFromDb.MarkedPhotos = photos;
            
            var tags = new List<Models.Tag>(photoFromDb.Tags);
            tags.Remove(tagFromDb);
            photoFromDb.Tags = tags;
            
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

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}