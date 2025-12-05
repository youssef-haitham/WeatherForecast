namespace WeatherForecast.Api.Interfaces.Providers
{
    public interface IHashProvider
    {
        string HashPassword(string password);

        bool Verify(string password, string hashed);
    }
}
