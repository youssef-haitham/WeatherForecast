using WeatherForecast.Api.Entities;
using WeatherForecast.Api.Enums;
using WeatherForecast.Api.Interfaces.Repositories;

namespace WeatherForecast.Api.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private static readonly WeatherCondition[] Conditions = Enum.GetValues<WeatherCondition>();
        public Weather GetByCity(string city)
        {
            return new Weather
            {
                City = city,
                Temperature = Random.Shared.Next(15, 40),
                Condition = Conditions[Random.Shared.Next(Conditions.Length)]
            };
        }
    }
}