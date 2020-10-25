using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoFlicker.Application.Repository.Tag;
using PhotoFlicker.Models;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private readonly ITagRepository _repository;

        public TagController(ITagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{amount}")]
        public async Task<ActionResult<IEnumerable<Tag>>> Take([FromRoute] int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być ujemna"); }

            var data = await _repository.Take(amount);
            return Ok(data);
        }

        [HttpGet]
        [Route("random/{amount}")]
        public async Task<ActionResult<IEnumerable<Tag>>> TakeRandom([FromRoute] int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może być ujemna"); }

            var data = await _repository.GetRandom(amount);
            return Ok(data);
        }
        
        [HttpGet]
        [Route("single/{name}")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetByName([FromRoute] string name)
        {
            var fromRepo = await _repository.GetByNameLike(name);
            if (fromRepo != null)
            {
                return Ok(fromRepo);
            }

            return BadRequest("Tag o podanym id nie istnieje");
        }
        
        [HttpGet]
        [Route("canFind/{name}")]
        public async Task<ActionResult<IEnumerable<Tag>>> IsTagExists([FromRoute] string name)
        {
            var fromRepo = await _repository.GetByNameLike(name);
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
            var output = await _repository.IsTagNameExist(name);
            return Ok(output);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<bool>> Create()
        {
            var created = new Tag();
            if (created == null)
            {
                return BadRequest("Argument \"created\" cannot be null");
            }

            if (await _repository.Create(created))
            {
                await _repository.SaveChanges();
                return Ok(true);
            }

            return BadRequest("Error");
        }
    }
}