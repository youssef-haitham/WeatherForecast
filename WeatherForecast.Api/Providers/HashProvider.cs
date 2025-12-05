using WeatherForecast.Api.Interfaces.Providers;

namespace WeatherForecast.Api.Providers
{
    public class HashProvider : IHashProvider
    {
        public string HashPassword(string password)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(password);
            return hashed;
        }

        public bool Verify(string password, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }
    }
}