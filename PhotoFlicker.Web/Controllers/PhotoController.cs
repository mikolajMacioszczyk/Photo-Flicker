using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoFlicker.Application.Service.Photo;
using PhotoFlicker.Models.Dtos.Photo;
using PhotoFlicker.Models.Dtos.Tag;

namespace PhotoFlicker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
        private readonly IPhotoService _service;

        public PhotoController(IPhotoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{amount}")]
        public async Task<ActionResult<IEnumerable<PhotoReadDto>>> Take([FromRoute] int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być liczbą ujemną"); }
            
            var data = await _service.TakeIncludeTags(amount);
            return Ok(data);
        }

        [HttpGet]
        [Route("{tagId}/{amount}")]
        public async Task<ActionResult<IEnumerable<PhotoReadDto>>> TakeWhereTag([FromRoute] int tagId, [FromRoute] int amount)
        { 
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być liczbą ujemną"); }

            if (!(await _service.IsTagExist(tagId))) { return BadRequest("Tag o podanym id nie istnieje"); }

            return Ok(await _service.TakeWhereTag(tagId, amount));
        }

        [HttpGet]
        [Route("tagLikes/{pattern}/{amount}")]
        public async Task<ActionResult<IEnumerable<TagReadDto>>> TakeWithTagLikes([FromRoute] string pattern, int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być liczbą ujemną"); }

            IEnumerable<PhotoReadDto> data;
            if (string.IsNullOrEmpty(pattern))
            {
                data = await _service.Take(amount);
            }
            else
            {
                data = await _service.TakeWithTagLike(pattern, amount);
            }
            
            return Ok(data);
        }

        [HttpPost]
        [Route("validate")]
        public async Task<ActionResult<(bool, string[])>> ValidateTagsAsPlainText([FromBody] string text)
        {
            var result = _service.ValidateTasksAsPlainText(text);
            if (result != null)
            {
                return Ok(result);
            }
            return Problem();
        }

    }
}