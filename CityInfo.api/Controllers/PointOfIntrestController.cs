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
        private readonly CitiesDataStore _citiesDataStore;

        public PointOfIntrestController(ILogger<PointOfIntrestController> logger, 
            IMailService mailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore)); 
        }
        [HttpGet]
        public ActionResult<IEnumerable<PointOfIntrestDto>> GetPointOfIntrest(int cityId)
        {
            try
            {
               
                var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of intrest.");
                    return NotFound();
                }
                return Ok(city.PointOfIntrest);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception while getting point of intrest for city with id {cityId}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
            
        }

        [HttpGet("{pointofintrestid}", Name = "GetPointOfIntrest")]

        public ActionResult<PointOfIntrestDto> GetPointOfIntrest(
            int cityId, int pointOfIntrestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }

            //find point of intrest
            var pointOfIntrest = city.PointOfIntrest.FirstOrDefault(c => c.Id == pointOfIntrestId);

            if(pointOfIntrest == null)
            {
                return NotFound();
            }
            return Ok(pointOfIntrest);
        }

        [HttpPost]
        public ActionResult<PointOfIntrestDto> CreatePointOfIntrest(int cityId, PointOfIntrestForCreationDto pointOfIntrest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if( city == null)
            {
                return NotFound();
            }
            // to be improved

            var maxPointOfIntrestId = _citiesDataStore.Cities.SelectMany(c => c.PointOfIntrest).Max(p => p.Id);

            var finalPointOfIntrest = new PointOfIntrestDto()
            {
                Id = ++maxPointOfIntrestId,
                Name = pointOfIntrest.Name,
                Description = pointOfIntrest.Description
            };

            city.PointOfIntrest.Add(finalPointOfIntrest);

            return CreatedAtRoute("GetPointOfIntrest", 
                new
                {
                    cityId = cityId,
                    pointOfIntrestId = finalPointOfIntrest.Id
                },
                finalPointOfIntrest);
        }

        [HttpPut("{pointofintrestid}")]

        public ActionResult UpdatePointOfIntrest(int cityId, int pointOfIntrestId, PointOfIntrestForUpdateDto pointOfIntrest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
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
