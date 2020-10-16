using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PhotoFlicker.Models;
using PhotoFlicker.Web.Db.Context;

namespace PhotoFlicker.Tests.RepositoryUnitTests.PhotoRepository
{
    [TestFixture]
    public class Delete : PhotoFlickerTestBase
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
        public async Task Delete_InvalidId_ShouldNotModifyCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Web.Db.Repository.Page.PhotoRepository(context);
            var idToDelete = 10;
            var expectedSize = Photos.Count;
            
            //act
            var result = await repository.Delete(idToDelete);
            await repository.SaveChanges();
            var allPhotos = await repository.Take(10);

            //assert
            Assert.False(result);
            Assert.AreEqual(expectedSize, allPhotos.Count());

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Delete_ValidId_SharedTag_ShouldRemove_ShouldNotRemoveAnyTag()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Web.Db.Repository.Page.PhotoRepository(context);
            var idToDelete = 3;
            var expectedSize = Photos.Count -1;
            
            //act
            var result = await repository.Delete(idToDelete);
            await repository.SaveChanges();
            var allPhotos = await repository.Take(10);

            //assert
            Assert.True(result);
            Assert.AreEqual(expectedSize, allPhotos.Count());
            Assert.Null(allPhotos.FirstOrDefault(p => p.Id == idToDelete));
            Assert.True(await repository.IsTagExist(3));

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Delete_ValidId_NotSharedTag_ShouldRemove_ShouldNotRemoveAnyTag()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Web.Db.Repository.Page.PhotoRepository(context);
            var idToDelete = 1;
            var expectedSize = Photos.Count -1;
            
            //act
            var result = await repository.Delete(idToDelete);
            await repository.SaveChanges();
            var allPhotos = await repository.Take(10);

            //assert
            Assert.True(result);
            Assert.AreEqual(expectedSize, allPhotos.Count());
            Assert.Null(allPhotos.FirstOrDefault(p => p.Id == idToDelete));
            Assert.True(await repository.IsTagExist(1));

            //clean
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