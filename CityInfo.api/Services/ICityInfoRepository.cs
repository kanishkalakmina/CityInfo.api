using CityInfo.api.Entities;

namespace CityInfo.api.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<City?> GetCityAsync(int cityId, bool includePointOfIntrest);

        Task<bool> CityExistsAsync(int cityId);

        Task<IEnumerable<PointOfIntrest?>> GetPointsOfIntrestForCityAsunc(int cityId, int pointOfIntrestId);

        Task<PointOfIntrest?> GetPointOfIntrestForCityAsunc(int cityId, int pointOfIntrestId);
        Task GetPointsOfIntrestForCityAsunc(int cityId, object pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfIntrest pointOfIntrest);

        Task <bool> SaveChangesAsync();
    }
}
