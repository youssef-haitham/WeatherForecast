using WeatherForecast.Api.Entities;

namespace WeatherForecast.Api.Interfaces.Services
{
    public interface IWeatherService
    {
        Weather GetWeather(string city);
    }
}