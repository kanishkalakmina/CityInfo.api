using CityInfo.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.api.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfIntrest> pointOfIntrest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options)
        {

        }

      //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      //  {
       //     optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
      //  }
    }
}
