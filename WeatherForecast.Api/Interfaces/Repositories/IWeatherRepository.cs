using WeatherForecast.Api.Entities;

namespace WeatherForecast.Api.Interfaces.Repositories
{
    public interface IWeatherRepository
    {
        Weather GetByCity(string city);
    }
}