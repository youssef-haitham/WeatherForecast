using WeatherForecast.Api.Enums;

namespace WeatherForecast.Api.Entities
{
    public class Weather
    {
        public string City { get; set; } = null!;
        public int Temperature { get; set; }
        public WeatherCondition Condition { get; set; }
        public DateOnly Date { get; set; }
    }
}