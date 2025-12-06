using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Api.Interfaces.Services;

namespace WeatherForecast.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IWeatherService weatherService) : ControllerBase
    {
        private readonly IWeatherService _weatherService = weatherService;

        [Authorize]
        [HttpGet]
        public IActionResult GetWeather([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City is required.");

            var weather = _weatherService.GetWeather(city);

            return Ok(weather);
        }
    }
}