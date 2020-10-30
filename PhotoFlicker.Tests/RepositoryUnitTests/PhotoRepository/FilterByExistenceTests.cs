using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PhotoFlicker.Application.Context;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Tests.RepositoryUnitTests.PhotoRepository
{
    [TestFixture]
    public class FilterByExistenceTests : PhotoFlickerTestBase
    {
        private static readonly List<Tag> Tags = new List<Tag>()
        {
            new Tag(){Id = 1, Name = "Warsaw"},
            new Tag(){Id = 2, Name = "Berlin"},
            new Tag(){Id = 3, Name = "Paris"},
        };
        
        [Test]
        public async Task FilterByExistence_NullAgument_ShouldReturnEmptyList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            var expected = new string[]{};
            
            //act
            var result = await repository.FilterByExistence(null);
            
            //assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task FilterByExistence_NoNameInDb_ShouldReturnEmptyList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            var input = new []{"Madrid","Tokyo", "London"};
            var expected = new string[]{};
            
            //act
            var result = await repository.FilterByExistence(input);
            
            //assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task FilterByExistence_EveryNameInDb_ShouldReturnTheSameCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);

            var input = new[] {"Paris", "Berlin", "Warsaw"};
            var expected = new[] {"warsaw", "berlin", "paris"}.OrderBy(s => s);
            
            //act
            var result = (await repository.FilterByExistence(input)).OrderBy(s => s).ToList();

            //assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task FilterByExistence_HalfPartInDb_Configuration1()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            var input = new[] {"Madrid","Paris","Los Angeles" , "Warsaw", "Tokyo"};
            var expected = new[] {"warsaw", "paris"}.OrderBy(s => s);
            
            //act
            var result = (await repository.FilterByExistence(input)).OrderBy(s => s);

            //assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task FilterByExistence_HalfPartInDb_Configuration2()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            var input = new[] {"Paris","Los Angeles" , "Tokyo", "Warsaw"};
            var expected = new[] {"warsaw", "paris"}.OrderBy(s => s);
            
            //act
            var result = (await repository.FilterByExistence(input)).OrderBy(s => s);

            //assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task FilterByExistence_DoubledValues_ResultCollectionShouldHaveUniqueValues()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            var input = new[] {"Paris","Warsaw" , "Paris", "Warsaw"};
            var expected = new[] {"warsaw", "paris"}.OrderBy(s => s);
            
            //act
            var result = (await repository.FilterByExistence(input)).OrderBy(s => s);

            //assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task FilterByExistence_NullInArguments_ShouldDiscardNull()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            var input = new[] {"Paris",null, "Warsaw"};
            var expected = new[] {"warsaw", "paris"}.OrderBy(s => s);
            
            //act
            var result = (await repository.FilterByExistence(input)).OrderBy(s => s);

            //assert
            Assert.NotNull(result);
            Assert.True(result.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        private async Task<PhotoFlickerContext> InitializeContext()
        {
            var context = CreateContext();
            var tags = new List<Tag>(Tags);
            
            await context.TagItems.AddRangeAsync(tags);
            await context.SaveChangesAsync();
            return context;
        }
    }
}