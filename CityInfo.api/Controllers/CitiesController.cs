using AutoMapper;
using CityInfo.api.Models;
using CityInfo.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.api.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
           _cityInfoRepository = cityInfoRepository ?? throw new Exception(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public CitiesDataStore CitiesDataStore { get; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfIntrestDto>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfIntrestDto>>(cityEntities));
        }

         [HttpGet("{id}")]
         public async Task<ActionResult<CityDto>> GetCity(int id, bool includePointOfIntrest = false) 
         {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointOfIntrest);

            if(city == null)
            {
                return NotFound();
            }
            if(includePointOfIntrest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CityWithoutPointOfIntrestDto>(city));
         }
           
    }
    }