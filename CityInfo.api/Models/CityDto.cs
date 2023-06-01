﻿namespace CityInfo.api.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int NumberOfPointsOfIntrest
        {
            get
            {
                return PointOfIntrest.Count;
            }
        }
        public ICollection<PointOfIntrestDto> PointOfIntrest { get; set; }
      = new List<PointOfIntrestDto>();
    }
}
