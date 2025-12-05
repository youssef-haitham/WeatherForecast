using WeatherForecast.Api.Entities;
using WeatherForecast.Api.Interfaces.Repositories;

namespace WeatherForecast.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> Users = [];
        public User AddUser(User user)
        {
            Users.Add(user);
            return user;
        }

        public User? GetUserByEmail(string email)
        {
            return Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User? GetUserById(Guid userId)
        {
            return Users.FirstOrDefault(u => u.Id == userId);
        }

        public bool UserExistsByEmail(string email)
        {
            return Users.Exists(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}