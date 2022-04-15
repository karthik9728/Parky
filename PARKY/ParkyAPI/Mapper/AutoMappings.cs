using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;

namespace ParkyAPI.Mapper
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
