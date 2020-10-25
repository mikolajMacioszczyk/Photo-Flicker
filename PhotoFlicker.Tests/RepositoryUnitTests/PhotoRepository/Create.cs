using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PhotoFlicker.Application.Context;
using PhotoFlicker.Models;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Tests.RepositoryUnitTests.PhotoRepository
{
    [TestFixture]
    public class Create : PhotoFlickerTestBase
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
        public async Task Create_NullValue_ShouldReturnFalse()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);

            //act
            var result = await repository.Create(null);
            await repository.SaveChanges();

            //assert
            Assert.False(result);  

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Create_IdAlreadyIn_ShouldNotCreate_ShouldReturnFalse()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            var toCreate = new Photo()
            {
                Id = 1,
                Path = "Test 1"
            };

            //act
            var result = await repository.Create(toCreate);
            await repository.SaveChanges();
            var all = await repository.Take(10);

            //assert
            Assert.False(result);
            Assert.Null(all.FirstOrDefault(p => p.Path == "Test 1"));

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Create_ShouldCreateNew()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            var toCreate = new Photo()
            {
                Path = "Test 1"
            };

            //act
            var result = await repository.Create(toCreate);
            await repository.SaveChanges();
            var all = await repository.Take(10);

            //assert
            Assert.True(result);
            Assert.NotNull(all.FirstOrDefault(p => p.Path == "Test 1"));

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Create_TagListNotEmpty_ShouldEraseTask()
        {    
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            var toCreate = new Photo()
            {
                Path = "Test 1",
                Tags = new List<Tag>(){new Tag(){Id = 10, Name = "Should be Erased"}}
            };

            //act
            var result = await repository.Create(toCreate);
            await repository.SaveChanges();
            var allPhotos = await repository.Take(10);

            //assert
            Assert.True(result);
            Assert.NotNull(allPhotos.FirstOrDefault(p => p.Path == "Test 1"));

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