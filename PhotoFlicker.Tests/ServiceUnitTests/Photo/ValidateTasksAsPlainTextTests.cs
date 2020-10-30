using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NUnit.Framework;
using PhotoFlicker.Application.Context;
using PhotoFlicker.Application.Repository.Photo;
using PhotoFlicker.Application.Service.Photo;
using PhotoFlicker.Models.Models;
using PhotoFlicker.Models.Profiles;

namespace PhotoFlicker.Tests.ServiceUnitTests.Photo
{
    [TestFixture]
    public class ValidateTasksAsPlainTextTests: PhotoFlickerTestBase
    {
        private static readonly List<Tag> Tags = new List<Tag>()
        {
            new Tag(){Id = 1, Name = "Warsaw"},
            new Tag(){Id = 2, Name = "Berlin"},
            new Tag(){Id = 3, Name = "Paris"},
        };
        
        [Test]
        public async Task ValidateTasksAsPlainText_NullArgument_ShouldReturnTrue_ShouldReturnEmptyCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new string[]{};
            
            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(null);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.NotNull(result.InvalidValues);
            Assert.True(result.InvalidValues.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_EmptyCollectionArgument_ShouldReturnTrue_ShouldReturnEmptyCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new string[]{};
            var input = "";

            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.NotNull(result.InvalidValues);
            Assert.True(result.InvalidValues.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_OnlyExistingTags_ShouldReturnTrue_ShouldReturnFullCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new string[] { };
            var input = "#Warsaw #Paris #Berlin";
            
            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.NotNull(result.InvalidValues);
            Assert.True(result.InvalidValues.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_OnlyExistingTags_DoubledValues_ShouldReturnTrue_ShouldReturnFullCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new string[] { };
            var input = "#Warsaw #Paris #Warsaw #Paris";
            
            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.NotNull(result.InvalidValues);
            Assert.True(result.InvalidValues.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_OnlyNonExistingTags_ShouldReturnFalse_ShouldReturnEmptyCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new []{"Ottawa", "Oslo", "Madrid"}.OrderBy(s => s);
            var input = "#Oslo #Ottawa #Madrid";
            
            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues.OrderBy(s => s);

            //assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_BothExistingAndNonExisting_Configuration1_ShouldReturnFalse()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new []{"Madrid", "Rio"}.OrderBy(s => s);
            var input = "#Madrid #Warsaw #Rio #Paris";
            
            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues.OrderBy(s => s);

            //assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_BothExistingAndNonExisting_Configuration2_ShouldReturnFalse()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var input = "#Warsaw #Berlin #Madrid #Oslo #Warsaw";
            var expected = new []{"Madrid", "Oslo"}.OrderBy(s => s);
            
            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues.OrderBy(s => s);

            //assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_InvalidSpacing_ShouldDiscardSpacing_ShouldReturnFalse()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var input = "    #Warsaw         #Madrid       #Paris     ";
            var expected = new []{"Madrid"}.OrderBy(s => s);
            
            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues.OrderBy(s => s);

            //assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_OnlyExistingTags_WithTagsWithoutHash_ShouldBeDiscarded_ShouldReturnFalse()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new[] {"Warsaw Paris"};
            var input = "#Warsaw Paris #Berlin";

            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues;

            //assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_FirstArgumentWithoutHash_ShouldReturnTrue()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new string[]{};
            var input = "Warsaw";

            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues;

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_FirstArgumentWithoutHash_OthersWithHash_ShouldReturnTrue()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new string[]{};
            var input = "Warsaw #Berlin #Paris #Warsaw";

            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues;

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_TwoHashOneByOne_ShouldDiscardIt_ShouldReturnTrue()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new string[]{};
            var input = "#Berlin ##Paris #Warsaw";

            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues;

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task ValidateTasksAsPlainText_DuplicateValues_ButWithDifferentCases_ShouldReturnTrue()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new PhotoRepository(context);

            var expected = new string[]{};
            var input = "#Berlin #berlin";

            var service = new PhotoService(repository, CreateMapper());
            
            //act
            var result = await service.ValidateTasksAsPlainText(input);
            var resultCollection = result.InvalidValues;

            //assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.NotNull(resultCollection);
            Assert.True(resultCollection.SequenceEqual(expected));
            
            //clean
            DisposeContext(context);
        }

        private IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PhotoProfile>();
                cfg.AddProfile<TagProfile>();
            });
            return configuration.CreateMapper();
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