using System.Text.Json.Serialization;

namespace WeatherForecast.Api.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WeatherCondition
    {
        Sunny,
        Cloudy,
        Rainy,
        Windy,
        Stormy
    }
}