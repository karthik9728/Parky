using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : Controller
    {
        private INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository,IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;   
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var objList = _nationalParkRepository.GetNationalParks();
            var objDto = new List<NationalParkDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            return Ok(objDto);
        }

        [HttpGet("{id:int}",Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int id)
        {
            var obj = _nationalParkRepository.GetNationalPark(id);
            if(obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("","National Park Already Exists");
                return StatusCode(404,ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("",$"Something went worng while saving the record {nationalParkObj.Name}");
                return StatusCode(500,ModelState);  
            }

            return Ok(nationalParkObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int id,[FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || id!=nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went worng while updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int id)
        {
            if (!_nationalParkRepository.NationalParkExists(id)) 
            {
                return NotFound();
            }
            var nationalParkObj = _nationalParkRepository.GetNationalPark(id);

            if (!_nationalParkRepository.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went worng while deleting the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
