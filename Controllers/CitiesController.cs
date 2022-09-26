using System.Text.Json;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities")]
    [Authorize]
    public class CitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if(pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }
            var (cityEntities, PaginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
            Response.Headers.Add("X_Pagination", JsonSerializer.Serialize(PaginationMetadata));
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }
        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="id">The id of the city</param>
        /// <param name="includePointsOfInterest">If true, the points of interest will be returned</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="404">If the city is not found</response>
        /// <response code="400">If the city was not allowed.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if(city == null)
            {
                return NotFound();
            }

            if(includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            else
            {
                return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
            }
        }
    }
}
