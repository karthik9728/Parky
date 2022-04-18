using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs.Trail;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trail")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrail")]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get List of Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepository.GetTrails();
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get Individual Trail
        /// </summary>
        /// <param name="id">Id of the Trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var obj = _trailRepository.GetTrail(id);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Get Trail By National Park ID
        /// </summary>
        /// <param name="nationalParkId">Id of National Park</param>
        /// <returns>Trail in National Park</returns>
        [HttpGet("GetTrailInNationalPark/{nationalParkId:int}", Name = "GetTrailInNationalPark")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepository.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Create New Trail
        /// </summary>
        /// <param name="trailDto">DTO Type of Trail</param>
        /// <returns>New Trail Data</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] CreateTrailDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Already Exists in Database");
                return StatusCode(404, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went worng while creating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok(trailObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int id, [FromBody] UpdateTrailDto trailDto)
        {
            if (trailDto == null || id != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went worng while updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_trailRepository.TrailExists(id))
            {
                return NotFound();
            }
            var trailObj = _trailRepository.GetTrail(id);

            if (!_trailRepository.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went worng while deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
