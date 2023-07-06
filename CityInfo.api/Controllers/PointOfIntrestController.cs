using AutoMapper;
using CityInfo.api.Models;
using CityInfo.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.api.Controllers
{
    [Route("api/cities/{cityId}/pointofintrest")]
    [ApiController]
    public class PointOfIntrestController : ControllerBase
    {
        private readonly ILogger<PointOfIntrestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        private readonly CitiesDataStore _citiesDataStore;

        public PointOfIntrestController(ILogger<PointOfIntrestController> logger, 
            IMailService mailService, ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));    
            
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfIntrestDto>>> GetPointOfIntrest(int cityId)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found when accessing points of intrest. ");
                return NotFound();
            }
            var pointOfIntrestForCity = await _cityInfoRepository
                .GetPointsOfIntrestForCityAsunc(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfIntrestDto>>(pointOfIntrestForCity));
        }

        [HttpGet("{pointofintrestid}", Name = "GetPointOfIntrest")]

        public async Task<ActionResult<PointOfIntrestDto>> GetPointOfIntrest(
            int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfIntrestForCity = await _cityInfoRepository
               .GetPointsOfIntrestForCityAsunc(cityId, pointOfInterestId);

            if(pointOfInterestId == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<PointOfIntrestDto>>(pointOfIntrestForCity));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfIntrestDto>> CreatePointOfIntrest(int cityId, PointOfIntrestForCreationDto pointOfIntrest)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            // to be improved

            var finalPointOfIntrest = _mapper.Map<Entities.PointOfIntrest>(pointOfIntrest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfIntrest);

            var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfIntrestDto>(finalPointOfIntrest);
            await _cityInfoRepository.SaveChangesAsync();

            return CreatedAtRoute("GetPointOfIntrest", 
                new
                {
                    cityId = cityId,
                    pointOfIntrestId = createdPointOfInterestToReturn.Id
                },
                createdPointOfInterestToReturn);
        }

        [HttpPut("{pointofintrestid}")]

        public async Task<ActionResult> UpdatePointOfIntrest(int cityId, int pointOfIntrestId, PointOfIntrestForUpdateDto pointOfIntrest)
        {
           if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            //find point of intrest
            var pointOfIntrestFromStore = city.PointOfIntrest.FirstOrDefault(c => c.Id == pointOfIntrestId);

            if (pointOfIntrestFromStore == null)
            {
                return NotFound();
            }

            pointOfIntrestFromStore.Name = pointOfIntrest.Name;
            pointOfIntrestFromStore.Description = pointOfIntrest.Description;

            return NoContent();
        }

        [HttpPatch("pointofintrestid")]

        public ActionResult PartiallyUpdatePointOfIntrest( int cityId, int pointOfIntrestId, JsonPatchDocument<PointOfIntrestForUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfIntrestFromStore = city.PointOfIntrest.FirstOrDefault(c => c.Id == pointOfIntrestId);

            if (pointOfIntrestFromStore == null)
            {
                return NotFound();
            }

            var pointOfIntrestToPatch =
                new PointOfIntrestForUpdateDto()
                {
                    Name = pointOfIntrestFromStore.Name,
                    Description = pointOfIntrestFromStore.Description
                };
            patchDocument.ApplyTo(pointOfIntrestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfIntrestFromStore.Name = pointOfIntrestToPatch.Name;
            pointOfIntrestFromStore.Description = pointOfIntrestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfIntrestId}")]

        public ActionResult DeletePointOfIntrest( int cityId, int pointOfIntrestId )
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfIntrestFromStore = city.PointOfIntrest.FirstOrDefault(c => c.Id == pointOfIntrestId);

            if (pointOfIntrestFromStore == null)
            {
                return NotFound();
            }

            city.PointOfIntrest.Remove(pointOfIntrestFromStore);
            _mailService.Send(
                "Point of intrest deleted.",
                $"Point of intrest {pointOfIntrestFromStore.Name} with id {pointOfIntrestFromStore.Id} was deleted");
            return NoContent();
        }
    }
}
