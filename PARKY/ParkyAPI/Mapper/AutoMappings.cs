using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Models.DTOs.Trail;

namespace ParkyAPI.Mapper
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, CreateTrailDto>().ReverseMap();
            CreateMap<Trail, UpdateTrailDto>().ReverseMap();

        }
    }
}
