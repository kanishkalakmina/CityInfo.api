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
        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {
           
            //collection to start from
            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery)
                || (a.Description != null && a.Description.Contains(searchQuery)));
            }
            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn =  await collection.OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<IEnumerable<PointOfIntrest?>> GetPointsOfIntrestForCityAsync(int cityId)
        {
            return await _context.pointOfIntrest.Where(p => p.Id == cityId).ToListAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }


        public async Task<PointOfIntrest?> GetPointOfIntrestForCityAsync(int cityId, int pointOfIntrestId)
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

        public void DeletePointOfIntrest(PointOfIntrest pointOfIntrest)
        {
            _context.pointOfIntrest.Remove(pointOfIntrest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public Task<IEnumerable<PointOfIntrest?>> GetPointsOfIntrestForCityAsync(int cityId, int pointOfIntrestId)
        {
            throw new NotImplementedException();
        }

        public Task GetPointsOfIntrestForCityAsync(int cityId, object pointOfInterestId)
        {
            throw new NotImplementedException();
        }

        public void DeletePointOfInterestForCityAsync(PointOfIntrest pointOfIntrest)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
        {
            return await _context.Cities.AllAsync(c => c.Id == cityId && c.Name == cityName);
        }

        public Task GetPointOfIntrestForCityAsync(int cityId)
        {
            throw new NotImplementedException();
        }
    }
}
