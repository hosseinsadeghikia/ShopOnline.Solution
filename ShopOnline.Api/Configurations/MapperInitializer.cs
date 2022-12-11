using AutoMapper;
using ShopOnline.Core.DTOs;
using ShopOnline.Data;

namespace ShopOnline.Api.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            //CreateMap<Country, CountryDTO>().ReverseMap();
            //CreateMap<Country, CreateCountryDTO>().ReverseMap();
            //CreateMap<Country, UpdateCountryDTO>().ReverseMap();

            //CreateMap<Hotel, HotelDTO>().ReverseMap();
            //CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
            //CreateMap<Hotel, UpdateHotelDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
