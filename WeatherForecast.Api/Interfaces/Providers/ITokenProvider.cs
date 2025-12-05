namespace WeatherForecast.Api.Interfaces.Providers
{
    public interface ITokenProvider
    {
        string CreateToken(Guid id, string email);
        bool ValidateToken(string token);
    }
}