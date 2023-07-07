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
            var pointOfIntrestEntity = _cityInfoRepository
                .GetPointOfIntrestForCityAsunc(cityId, pointOfIntrestId);

            if (pointOfIntrestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfIntrest, pointOfIntrestEntity);

            

            return NoContent();
        }

        [HttpPatch("pointofintrestid")]

        public async  Task<ActionResult> PartiallyUpdatePointOfIntrest( int cityId, int pointOfIntrestId, JsonPatchDocument<PointOfIntrestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfIntrestEntity = await _cityInfoRepository.GetPointsOfIntrestForCityAsunc(cityId, pointOfIntrestId);
            if (pointOfIntrestEntity == null)
            {
                return NotFound();
            }
            var pointOfIntrestToPatch = _mapper.Map<PointOfIntrestForUpdateDto>(
                pointOfIntrestEntity);
        
            patchDocument.ApplyTo(pointOfIntrestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!TryValidateModel(pointOfIntrestToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(pointOfIntrestToPatch, pointOfIntrestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfIntrestId}")]

        public async Task<ActionResult> DeletePointOfIntrest( int cityId, int pointOfIntrestId )
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfIntrestEntity = await _cityInfoRepository.GetPointsOfIntrestForCityAsunc(cityId, pointOfIntrestId);
            if (pointOfIntrestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfIntrest(pointOfIntrestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send(
                "Point of intrest deleted.",
                $"Point of intrest {pointOfIntrestEntity.Name} with id {pointOfIntrestEntity.Id} was deleted");
            return NoContent();
        }
    }
}
