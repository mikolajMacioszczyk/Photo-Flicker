using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PhotoFlicker.Application.Context;
using PhotoFlicker.Models;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Tests.RepositoryUnitTests.TagRepository
{
    [TestFixture]
    public class Read : PhotoFlickerTestBase
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
        public async Task Take_NegativeArgument_ShouldReturnEmptyList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Tag.TagRepository(context);
            var argument = -5;
            var expectedSize = 0;
            
            //act
            var result = await repository.Take(argument);

            //assert
            Assert.AreEqual(expectedSize, result.Count());
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Take_ValidArgument_InCollectionRange_ShouldReturnEmptyList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Tag.TagRepository(context);
            var argument = 2;
            var expectedSize = 2;
            
            //act
            var result = await repository.Take(argument);

            //assert
            Assert.AreEqual(expectedSize, result.Count());
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Take_ValidArgument_MoreThanCollectionRange_ShouldReturnEmptyList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Tag.TagRepository(context);
            var argument = 10;
            var expectedSize = 3;
            
            //act
            var result = await repository.Take(argument);

            //assert
            Assert.AreEqual(expectedSize, result.Count());
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task GetRandom_TooHighArgument_ShouldReturnFullList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Tag.TagRepository(context);
            var argument = 10;
            var expectedSize = 3;
            
            //act
            var result = await repository.GetRandom(argument);

            //assert
            Assert.AreEqual(expectedSize, result.Count());
            
            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task GetByName_NullArgument_ShouldReturnNull()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Tag.TagRepository(context);
            
            //act
            var result = await repository.GetByNameLike(null);

            //assert
            Assert.Null(result);
            
            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task GetByName_MatchedArgument()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Tag.TagRepository(context);
            string name = "Warsaw";
            int expectedId = 1;
            
            //act
            var result = await repository.GetByNameLike(name);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedId, result.Id);
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task GetByName_ValidArgument_BadLetterCase_ShouldReturnPropertyObject()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Tag.TagRepository(context);
            string name = "waRSaw";
            int expectedId = 1;
            
            //act
            var result = await repository.GetByNameLike(name);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedId, result.Id);
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task GetByName_NotMatchedArgument_ShouldReturnNull()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Tag.TagRepository(context);
            string name = "Not Matched";
            
            //act
            var result = await repository.GetByNameLike(name);

            //assert
            Assert.Null(result);
            
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