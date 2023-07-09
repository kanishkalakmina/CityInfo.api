using CityInfo.api.Entities;

namespace CityInfo.api.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);

        Task<City?> GetCityAsync(int cityId, bool includePointOfIntrest);

        Task<bool> CityExistsAsync(int cityId);

        Task<IEnumerable<PointOfIntrest?>> GetPointsOfIntrestForCityAsync(int cityId, int pointOfIntrestId);

        Task<PointOfIntrest?> GetPointOfIntrestForCityAsync(int cityId, int pointOfIntrestId);
        Task GetPointsOfIntrestForCityAsync(int cityId, object pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfIntrest pointOfIntrest);

        void DeletePointOfInterestForCityAsync(PointOfIntrest pointOfIntrest);

        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);

        Task <bool> SaveChangesAsync();
        Task GetPointOfIntrestForCityAsync(int cityId);
    }
}
