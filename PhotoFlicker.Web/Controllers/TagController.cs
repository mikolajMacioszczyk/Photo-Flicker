using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoFlicker.Models;
using PhotoFlicker.Web.Db.Repository.Tag;

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
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może bym ujemna"); }

            var data = await _repository.Take(amount);
            return Ok(data);
        }
        
        [HttpGet]
        [Route("{photoId}/{amount}")]
        public async Task<ActionResult<IEnumerable<Tag>>> TakeWherePhoto([FromRoute] int photoId, [FromRoute] int amount)
        {
            if (amount < 0) { return BadRequest("Ilość pobranych elementów nie może bym ujemna"); }
            if (!(await _repository.IsPhotoExist(photoId))) {return BadRequest("Photo o tym id nie istnieje");}

            var data = await _repository.TakeWherePhoto(photoId, amount);
            return Ok(data);
        }
        
        [HttpGet]
        [Route("single/{name}")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetByName([FromRoute] string name)
        {
            var fromRepo = await _repository.GetByName(name);
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
            var fromRepo = await _repository.GetByName(name);
            if (fromRepo != null)
            {
                return Ok(true);
            }

            return Ok(false);
        }
    }
}