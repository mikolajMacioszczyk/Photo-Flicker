using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoFlicker.Application.Service.Tag;
using PhotoFlicker.Models.Dtos.Tag;

namespace PhotoFlicker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{amount}")]
        public async Task<ActionResult<IEnumerable<TagReadDto>>> Take([FromRoute] int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być ujemna"); }

            var data = await _service.Take(amount);
            return Ok(data);
        }

        [HttpGet]
        [Route("random/{amount}")]
        public async Task<ActionResult<IEnumerable<TagReadDto>>> TakeRandom([FromRoute] int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być ujemna"); }

            var data = await _service.GetRandom(amount);
            return Ok(data);
        }
        
        [HttpGet]
        [Route("randomNames/{amount}")]
        public async Task<ActionResult<IEnumerable<string>>> TakeRandomTagNames([FromRoute] int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być ujemna"); }

            var data = await _service.GetRandomTagNames(amount);
            return Ok(data);
        }
        
        [HttpGet]
        [Route("single/{name}")]
        public async Task<ActionResult<IEnumerable<TagReadDto>>> GetByName([FromRoute] string name)
        {
            var fromRepo = await _service.GetByNameLike(name);
            if (fromRepo != null)
            {
                return Ok(fromRepo);
            }

            return BadRequest("Tag o podanym id nie istnieje");
        }
        
        [HttpGet]
        [Route("canFind/{name}")]
        public async Task<ActionResult<IEnumerable<TagReadDto>>> IsTagExists([FromRoute] string name)
        {
            var fromRepo = await _service.GetByNameLike(name);
            if (fromRepo != null)
            {
                return Ok(true);
            }

            return Ok(false);
        }
        
        [HttpGet]
        [Route("isUnique/{name}")]
        public async Task<ActionResult<bool>> IsUnique([FromRoute] string name)
        {
            var output = await _service.IsTagNameExist(name);
            return Ok(output);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<bool>> Create([FromBody] TagCreateDto created)
        {
            if (created == null)
            {
                return BadRequest("Argument \"created\" cannot be null");
            }

            if (await _service.Create(created))
            {
                await _service.SaveChanges();
                return Ok(true);
            }

            return BadRequest("Error");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<bool>> Delete([FromRoute] int id)
        {
            return Ok(true);
        }
    }
}