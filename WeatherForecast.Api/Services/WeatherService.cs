using Microsoft.Extensions.Caching.Memory;
using WeatherForecast.Api.Entities;
using WeatherForecast.Api.Interfaces.Repositories;
using WeatherForecast.Api.Interfaces.Services;

namespace WeatherForecast.Api.Services
{
    public class WeatherService(IWeatherRepository weatherRepo, IMemoryCache cache) : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepo = weatherRepo;
        private readonly IMemoryCache _cache = cache;

        public Weather GetWeather(string city)
        {
            string cacheKey = $"weather_{city.ToLower()}";

            if (_cache.TryGetValue(cacheKey, out Weather cachedWeather))
            {
                return cachedWeather;
            }

            var weather = _weatherRepo.GetByCity(city);

            _cache.Set(cacheKey, weather, TimeSpan.FromSeconds(30));

            return weather;
        }
    }
}