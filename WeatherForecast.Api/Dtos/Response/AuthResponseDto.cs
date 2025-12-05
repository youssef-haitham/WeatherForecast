namespace WeatherForecast.Api.Dtos.Response
{
    public class AuthResponseDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
