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
        public async Task Take_NegativeArgument_ShouldReturnEmptyCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            //act
            var result = await repository.Take(-5);
            
            //assert
            Assert.AreEqual(0, result.Count());
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Take_PositiveArgumentNotGreaterThanDataSize()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            //act
            var result = await repository.Take(2);
            
            //assert
            Assert.AreEqual(2, result.Count());
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task Take_PositiveArgumentGreaterThanDataSize_ShouldNotFail_ShouldReturnFullList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            //act
            var result = await repository.Take(10);
            
            //assert
            Assert.True(Photos.Select(p => p.Id).OrderBy(i => i)
                .SequenceEqual(result.Select(p => p.Id).OrderBy(i => i)));
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeIncludeTags_NegativeArgument_ShouldReturnEmptyCollection()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            //act
            var result = await repository.TakeIncludeTags(-5);
            
            //assert
            Assert.AreEqual(0, result.Count());
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeIncludeTags_PositiveArgumentNotGreaterThanDataSize()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            //act
            var result = await repository.TakeIncludeTags(2);
            var tagsOfFirstPhoto = result.FirstOrDefault(p => p.Id == 1).Tags;
            
            //assert
            Assert.AreEqual(2, result.Count());
            Assert.True(tagsOfFirstPhoto.Any(t => t.Id == 1));
            Assert.True(tagsOfFirstPhoto.Any(t => t.Id == 2));
            Assert.AreEqual(2, tagsOfFirstPhoto.Count());
            
            //clean
            DisposeContext(context);
        }

        [Test] public async Task TakeIncludeTags_PositiveArgumentGreaterThanDataSize_ShouldNotFail_ShouldReturnFullList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            
            //act
            var result = await repository.TakeIncludeTags(10);
            var tagsOfFirstPhoto = result.FirstOrDefault(p => p.Id == 1).Tags;

            //assert
            Assert.True(Photos.Select(p => p.Id).OrderBy(i => i)
                .SequenceEqual(result.Select(p => p.Id).OrderBy(i => i)));
            Assert.True(tagsOfFirstPhoto.Any(t => t.Id == 1));
            Assert.True(tagsOfFirstPhoto.Any(t => t.Id == 2));
            Assert.AreEqual(2, tagsOfFirstPhoto.Count());
            
            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task TakeWhereTag_NegativeTagId_ShouldReturnEmptyList()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int tagId = -1;
            int maxAmount = 2;
            
            //act
            var result = await repository.TakeWhereTag(tagId,maxAmount);

            //assert
            Assert.AreEqual(0, result.Count());
            
            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task TakeWhereTag_ValidTagId_AmountGreaterThanCollectionSize()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int tagId = 2;
            int expectedCount = 2;
            int maxAmount = 10;
            
            //act
            var result = await repository.TakeWhereTag(tagId,maxAmount);
            var firstResult = result.First(p => p.Id == 1);
            var secResult = result.First(p => p.Id == 2);

            //assert
            Assert.AreEqual(expectedCount, result.Count());
            Assert.NotNull(firstResult);
            Assert.NotNull(secResult);
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeWhereTag_ValidTagId_AmountLessThanCollectionSize()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int tagId = 2;
            int amount = 1;
            
            //act
            var result = await repository.TakeWhereTag(tagId,amount);
            var firstResult = result.First();

            //assert
            Assert.AreEqual(1, result.Count());
            Assert.NotNull(firstResult);
            Assert.True(firstResult.Tags.Any(t => t.Id == tagId));
            
            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task TakeRandom_NegativeArgument()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = -1;
            
            //act
            var result = await repository.TakeRandom(amount);

            //assert
            Assert.AreEqual(0, result.Count());
            
            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task TakeRandom_ValidArgument_ShouldReturnDifferentValues()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = 3;

            for (int i = 0; i < 10; i++)
            {
                //act
                var result = await repository.TakeRandom(amount);

                //assert
                Assert.AreEqual(amount, result.Count());  
                Assert.True(result.First().Id != result.Skip(1).First().Id);
                Assert.True(result.First().Id != result.Skip(2).First().Id);
                Assert.True(result.Skip(1).First().Id != result.Skip(2).First().Id);
            }

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeRandomWithTag_InvalidTagId()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = 10;
            int tagId = 10;
            
            //act
            var result = await repository.TakeRandomWithTag(tagId, amount);

            //assert
            Assert.AreEqual(0, result.Count());
            
            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeRandomWithTag_ValidTagId_NegativeAmount()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = -1;
            int tagId = 10;
            
            //act
            var result = await repository.TakeRandomWithTag(tagId, amount);

            //assert
            Assert.AreEqual(0, result.Count());
            
            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task TakeRandomWithTag_ValidArguments_FullListForThatTag()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int maxAmount = 3;
            int tagId = 2;

            for (int i = 0; i < 10; i++)
            {
                //act
                var result = await repository.TakeRandomWithTag(tagId, maxAmount);

                //assert
                Assert.AreEqual(2, result.Count());  
                Assert.True(result.First().Id != result.Skip(1).First().Id);
            }

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeRandomWithTag_ValidArguments()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = 1;
            int tagId = 2;

            for (int i = 0; i < 10; i++)
            {
                //act
                var result = await repository.TakeRandomWithTag(tagId, amount);

                //assert
                Assert.AreEqual(amount, result.Count());  
                Assert.True(result.First().Tags.Any(t => t.Id == tagId));
            }

            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task TakeWithTagLike_NullPattern()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = 2;
            int expectedSize = 2;

            for (int i = 0; i < 10; i++)
            {
                //act
                var result = await repository.TakeWithTagLike(null, amount);

                //assert
                Assert.AreEqual(2, result.Count());  
            }

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeWithTagLike_PatternFullMatch()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = 5;
            string pattern = "Warsaw";

            //act
            var result = await repository.TakeWithTagLike(pattern, amount);

            //assert
            Assert.AreEqual(1, result.Count());  
            Assert.True(result.First().Id == 1);  

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeWithTagLike_PatternNotMatch()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = 2;
            string pattern = "NotMatched";

            //act
            var result = await repository.TakeWithTagLike(pattern, amount);

            //assert
            Assert.AreEqual(0, result.Count());  

            //clean
            DisposeContext(context);
        }
        
        [Test]
        public async Task TakeWithTagLike_PatternMatchInTheMiddleOfString()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = 5;
            string pattern = "erli";

            //act
            var result = await repository.TakeWithTagLike(pattern, amount);

            //assert
            Assert.AreEqual(2, result.Count());  
            Assert.True(result.Any(p => p.Id == 1));  
            Assert.True(result.Any(p => p.Id == 2));  

            //clean
            DisposeContext(context);
        }

        [Test]
        public async Task TakeWithTagLike_PatternMatchMoreThanOne()
        {
            // arrange
            var context = await InitializeContext();
            var repository = new Application.Repository.Photo.PhotoRepository(context);
            int amount = 5;
            int expectedSize = 3;
            string pattern = "ar";

            //act
            var result = await repository.TakeWithTagLike(pattern, amount);

            //assert
            Assert.AreEqual(3, result.Count());  
            Assert.Null(result.FirstOrDefault(p => p.Id == 2));

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