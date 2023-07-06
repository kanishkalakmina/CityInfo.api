using AutoMapper;

namespace CityInfo.api.Profiles
{
    public class PointOfIntrestProfile : Profile
    {
        public PointOfIntrestProfile()
        {
            CreateMap<Entities.PointOfIntrest, Models.PointOfIntrestDto>();
            CreateMap<Models.PointOfIntrestForCreationDto, Entities.PointOfIntrest>();
            CreateMap<Models.PointOfIntrestForUpdateDto, Entities.PointOfIntrest>();
        }
    }
}
