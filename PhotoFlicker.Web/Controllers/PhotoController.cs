using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoFlicker.Application.Repository.Page;
using PhotoFlicker.Models;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _repository;

        public PhotoController(IPhotoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{amount}")]
        public async Task<ActionResult<IEnumerable<Photo>>> Take([FromRoute] int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być liczbą ujemną"); }
            
            var data = await _repository.TakeIncludeTags(amount);
            return Ok(data);
        }

        [HttpGet]
        [Route("{tagId}/{amount}")]
        public async Task<ActionResult<IEnumerable<Photo>>> TakeWhereTag([FromRoute] int tagId, [FromRoute] int amount)
        { 
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być liczbą ujemną"); }

            if (!(await _repository.IsTagExist(tagId))) { return BadRequest("Tag o podanym id nie istnieje"); }

            return Ok(await _repository.TakeWhereTag(tagId, amount));
        }

        [HttpGet]
        [Route("tagLikes/{pattern}/{amount}")]
        public async Task<ActionResult<IEnumerable<Tag>>> TakeWithTagLikes([FromRoute] string pattern, int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być liczbą ujemną"); }

            IEnumerable<Photo> data;
            if (string.IsNullOrEmpty(pattern))
            {
                data = await _repository.Take(amount);
            }
            else
            {
                data = await _repository.TakeWithTagLike(pattern, amount);
            }
            
            return Ok(data);
        }

    }
}