using CityInfo.api.DbContexts;
using CityInfo.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.api.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<City?> GetCityAsync(int cityId, bool includePointOfIntrest)
        {
            if (includePointOfIntrest)
            {
                return await _context.Cities.Include(c => c.pointOfIntrests).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
         return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<PointOfIntrest?>> GetPointsOfIntrestForCityAsunc(int cityId)
        {
            return await _context.pointOfIntrest.Where(p => p.Id == cityId).ToListAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<PointOfIntrest?> GetPointOfIntrestForCityAsunc(int cityId, int pointOfIntrestId)
        {
            return await _context.pointOfIntrest.Where(p => p.Id == cityId && p.Id == pointOfIntrestId).FirstOrDefaultAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfIntrest pointOfIntrest)
        {
            var city = await GetCityAsync(cityId, false);
            if(city != null)
            {
                city.pointOfIntrests.Add(pointOfIntrest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
