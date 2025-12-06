using NUnit.Framework;
using WeatherForecast.Api.Entities;
using WeatherForecast.Api.Repositories;

namespace WeatherForecast.Api.Tests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private UserRepository UserRepository;

        [SetUp]
        public void Setup()
        {
            UserRepository = new UserRepository();
        }

        [Test]
        public void AddUser_ShouldStoreUserCorrectly()
        {
            var user = new User
            {
                Name = "Test",
                Email = "test@test.com",
                PasswordHash = "HASHED"
            };

            var addedUser = UserRepository.AddUser(user);

            Assert.That(addedUser, Is.Not.Null);
            Assert.That(addedUser.Email, Is.EqualTo("test@test.com"));
        }

        [Test]
        public void GetUserByEmail_ShouldReturnUser_WhenFound()
        {
            var user = new User
            {
                Name = "Test",
                Email = "test@test.com",
                PasswordHash = "HASHED"
            };

            UserRepository.AddUser(user);

            var result = UserRepository.GetUserByEmail("test@test.com");

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Email, Is.EqualTo("test@test.com"));
        }

        [Test]
        public void GetUserByEmail_ShouldReturnNull_WhenNotFound()
        {
            var result = UserRepository.GetUserByEmail("missing@test.com");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetUserById_ShouldReturnUser_WhenFound()
        {
            var user = new User
            {
                Name = "Test",
                Email = "test@test.com",
                PasswordHash = "HASHED"
            };

            var added = UserRepository.AddUser(user);

            var result = UserRepository.GetUserById(added.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(added.Id));
        }

        [Test]
        public void GetUserById_ShouldReturnNull_WhenNotFound()
        {
            var result = UserRepository.GetUserById(Guid.NewGuid());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void UserExistsByEmail_ShouldReturnTrue_WhenEmailExists()
        {
            var user = new User
            {
                Name = "Test",
                Email = "exists@test.com",
                PasswordHash = "HASHED"
            };

            UserRepository.AddUser(user);

            var exists = UserRepository.UserExistsByEmail("exists@test.com");

            Assert.That(exists, Is.True);
        }

        [Test]
        public void UserExistsByEmail_ShouldReturnFalse_WhenEmailNotExists()
        {
            var exists = UserRepository.UserExistsByEmail("none@test.com");

            Assert.That(exists, Is.False);
        }
    }
}