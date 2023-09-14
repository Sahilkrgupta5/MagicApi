using MagicApi.DataStore;
using MagicApi.Logging;
using MagicApi.Models;
using MagicApi.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicApi.Controllers
{
    [Route("api/MagicApi")]
    [ApiController]
    public class MagicApiController : ControllerBase
    {
        private readonly ILogging _logger;

        public MagicApiController(ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MagicDto>> GetMagics()
        {
            _logger.Log("Getting All Magices","");
            return Ok(MagicStore.magicList);
        }

        [HttpGet("{id:int}",Name ="GetMagices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MagicDto> GetMagices(int id)
        {
            if (id == 0)
            {
                _logger.Log("Get Magic Error with Id " + id,"error");
                return BadRequest();
            }
            var magic = MagicStore.magicList.FirstOrDefault(u => u.Id == id);
            if (magic == null)
            {
                return NotFound();
            }
            return Ok(magic);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MagicDto> CreateMagic([FromBody]MagicDto magicDto)
        {
            if (MagicStore.magicList.FirstOrDefault(u=>u.Name.ToLower()==magicDto.Name.ToLower())!=null)
            {
                ModelState.AddModelError("CustomError", "Name already exists!");
                return BadRequest(ModelState);
            }
            if(magicDto==null)
            {
                return BadRequest();
            }
            if(magicDto.Id>0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            magicDto.Id = MagicStore.magicList.OrderByDescending(u => u.Id).FirstOrDefault().Id+1;
            MagicStore.magicList.Add(magicDto);
            
            return CreatedAtRoute("GetMagices",new { id = magicDto.Id },magicDto);
        }
        [HttpDelete("{id:int}",Name ="DeleteMagices")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteMagic(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var magic = MagicStore.magicList.FirstOrDefault(u => u.Id ==id);
            if(magic==null)
            {
                return NotFound();
            }
            MagicStore.magicList.Remove(magic);
            return NoContent();
        }
        [HttpPut("{id:int}",Name ="UpdateMagices")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateMagic(int id, [FromBody]MagicDto magicDto)
        {
            if(magicDto == null || id != magicDto.Id)
            {
                return BadRequest();
            }
            var magic = MagicStore.magicList.FirstOrDefault(u => u.Id == id);
            magic.Name = magicDto.Name;
            magic.Occupancy = magicDto.Occupancy;
            magic.Sqft = magicDto.Sqft;

            return NoContent();
        }
        [HttpPatch("{id:int}", Name ="UpdatePartialMagic")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialMagic(int id, JsonPatchDocument<MagicDto> patchDto)
        {
            if(patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var magic = MagicStore.magicList.FirstOrDefault(u => u.Id == id);
            if(magic == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(magic, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
