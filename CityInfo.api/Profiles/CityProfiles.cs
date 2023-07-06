using AutoMapper;

namespace CityInfo.api.Profiles
{
    public class CityProfiles : Profile
    {
        public CityProfiles() 
        { 
            CreateMap<Entities.City, Models.CityWithoutPointOfIntrestDto>();
        }
    }
}
