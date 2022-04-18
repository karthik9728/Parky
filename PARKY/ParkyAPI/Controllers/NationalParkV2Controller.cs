using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalpark")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    public class NationalParkV2Controller : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get List of National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var obj = _nationalParkRepository.GetNationalParks().FirstOrDefault();
            return Ok(_mapper.Map<NationalParkDto>(obj));
        }
    }
}
