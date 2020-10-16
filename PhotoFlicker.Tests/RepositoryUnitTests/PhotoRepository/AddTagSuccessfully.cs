using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PhotoFlicker.Models;
using PhotoFlicker.Web.Db.Context;

namespace PhotoFlicker.Tests.RepositoryUnitTests.PhotoRepository
{
    [TestFixture]
    public class AddTagSuccessfully : PhotoFlickerTestBase
    {
        private static readonly List<Tag> Tags = new List<Tag>()
        {
            new Tag(){Id = 1, Name = "Warsaw"},
            new Tag(){Id = 2, Name = "Berlin"},
            new Tag(){Id = 3, Name = "Paris"},
        };
        private static readonly List<Photo> Photos = new List<Photo>()
        {
            new Photo(){Id = 1, Path = "Test Path 1", Tags = new List<Tag>(){Tags[0], Tags[1]}},
            new Photo(){Id = 2, Path = "Test Path 2", Tags = new List<Tag>(){Tags[1]}},
            new Photo(){Id = 3, Path = "Test Path 3", Tags = new List<Tag>(){Tags[2]}},
            new Photo(){Id = 4, Path = "Test Path 4", Tags = new List<Tag>(){Tags[2]}},
        };
        
        [Test]
        public async Task AddTagToPhoto_ValidTagId_ShouldAddTag_ShouldReturnTrue()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Web.Db.Repository.Page.PhotoRepository(context);
            var tagId = 3;
            var photoId = 1;
            
            //act
            var result = await repository.AddTagToPhoto(photoId, tagId);
            await repository.SaveChanges();
            var photoFromDb = await repository.GetById(photoId);
        
            //assert
            Assert.True(result);
            Assert.NotNull(photoFromDb.Tags.FirstOrDefault(t => t.Id == tagId));
        
            //clean
            photoFromDb.Tags = Photos.First(p => p.Id == photoId).Tags;
            await repository.SaveChanges();
            DisposeContext(context);
        }
        
        private async Task<PhotoFlickerContext> InitializeContext()
        {
            var context = CreateContext();
            var tags = new List<Tag>(Tags);
            var photos = new List<Photo>(Photos);
            
            await context.TagItems.AddRangeAsync(tags);
            await context.PhotoItems.AddRangeAsync(photos);
            await context.SaveChangesAsync();
            return context;
        }
    }
}