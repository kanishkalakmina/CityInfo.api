using CityInfo.api.Models;

namespace CityInfo.api
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

       // public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore() { 
        
        //init dummy data
        Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Test",
                    Description = "Test",
                    PointOfIntrest = new List<PointOfIntrestDto>()
                    {
                        new PointOfIntrestDto()
                        {
                            Id= 1,
                            Name= "Test",
                            Description = "Test",
                        },
                         new PointOfIntrestDto()
                        {
                            Id= 2,
                            Name= "Test",
                            Description = "Test",
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Test",
                    Description = "Test",
                      PointOfIntrest = new List<PointOfIntrestDto>()
                    {
                        new PointOfIntrestDto()
                        {
                            Id= 3,
                            Name= "Test",
                            Description = "Test",
                        },
                         new PointOfIntrestDto()
                        {
                            Id= 4,
                            Name= "Test",
                            Description = "Test",
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Test",
                    Description = "Test",
                      PointOfIntrest = new List<PointOfIntrestDto>()
                    {
                        new PointOfIntrestDto()
                        {
                            Id= 5,
                            Name= "Test",
                            Description = "Test",
                        },
                         new PointOfIntrestDto()
                        {
                            Id= 6,
                            Name= "Test",
                            Description = "Test",
                        }
                    }
                }
            };
        
        }
    }
}
