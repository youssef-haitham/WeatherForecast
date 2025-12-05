using WeatherForecast.Api.Entities;

namespace WeatherForecast.Api.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User AddUser(User user);
        User? GetUserById(Guid userId);
        User? GetUserByEmail(string email);
        bool UserExistsByEmail(string email);
    }
}