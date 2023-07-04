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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
            new City("New York City")
            {
                Id = 1,
                Description = "The one with that big park"
            },
             new City("Antwerp")
             {
                 Id = 2,
                 Description = "The one with the cathedral that was never really finished"
             },
              new City("Paris")
              {
                  Id = 3,
                  Description = "The one with that big tower"
              });

            modelBuilder.Entity<PointOfIntrest>().HasData(
           new PointOfIntrest("central park")
           {
               Id = 1,
               CityId = 1,
               Description = "The most visited park in the US"
           },
            new PointOfIntrest("Empire State Building")
            {
                Id = 2,
                CityId = 1,
                Description = "A 102-story skyscraper located in midtown manhattan"
            },
             new PointOfIntrest("cathedral")
             {
                 Id = 3,
                 CityId = 2,
                 Description = "The numaric hellos"
             });
            base.OnModelCreating(modelBuilder);
        }

        //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //  {
        //     optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //  }
    }
}
