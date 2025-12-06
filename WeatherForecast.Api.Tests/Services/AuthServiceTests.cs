using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using WeatherForecast.Api.Dtos.Request;
using WeatherForecast.Api.Entities;
using WeatherForecast.Api.Interfaces.Providers;
using WeatherForecast.Api.Interfaces.Repositories;
using WeatherForecast.Api.Services;

namespace WeatherForecast.Api.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> UserRepoMock;
        private Mock<IHashProvider> HashProviderMock;
        private Mock<ITokenProvider> TokenProviderMock;

        private AuthService AuthService;

        [SetUp]
        public void Setup()
        {
            UserRepoMock = new Mock<IUserRepository>();
            HashProviderMock = new Mock<IHashProvider>();
            TokenProviderMock = new Mock<ITokenProvider>();

            AuthService = new AuthService(UserRepoMock.Object, TokenProviderMock.Object, HashProviderMock.Object);
        }

        [Test]
        public void SignUp_ShouldThrow_WhenEmailAlreadyExists()
        {
            var signup = new SignUpRequestDto
            {
                Name = "Test",
                Email = "test@test.com",
                Password = "123"
            };

            UserRepoMock
                .Setup(r => r.UserExistsByEmail(signup.Email))
                .Returns(true);

            Assert.Throws<Exception>(() => AuthService.SignUp(signup));
        }

        [Test]
        public void SignUp_ShouldReturnAuthResponse_WhenSuccessful()
        {
            var req = new SignUpRequestDto
            {
                Name = "Youssef",
                Email = "youssef@test.com",
                Password = "123456"
            };

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                Email = req.Email,
                PasswordHash = "HASHED"
            };

            UserRepoMock.Setup(r => r.UserExistsByEmail(req.Email)).Returns(false);
            HashProviderMock.Setup(h => h.HashPassword(req.Password)).Returns("HASHED");
            UserRepoMock
                .Setup(r => r.AddUser(It.IsAny<User>()))
                .Returns((User u) =>
                {
                    u.Id = newUser.Id;
                    return u;
                });
            TokenProviderMock
                .Setup(t => t.CreateToken(newUser.Id, newUser.Email))
                .Returns("TOKEN");

            var (response, token) = AuthService.SignUp(req);

            Assert.That(response.Email, Is.EqualTo(req.Email));
            Assert.That(token, Is.EqualTo("TOKEN"));
        }

        [Test]
        public void SignIn_ShouldThrow_WhenEmailNotFound()
        {
            var signin = new SignInRequestDto
            {
                Email = "missing@test.com",
                Password = "123"
            };

            UserRepoMock
                .Setup(r => r.GetUserByEmail(signin.Email))
                .Returns((User?)null);

            Assert.Throws<BadHttpRequestException>(() => AuthService.SignIn(signin));
        }

        [Test]
        public void SignIn_ShouldThrow_WhenPasswordInvalid()
        {
            var signin = new SignInRequestDto
            {
                Email = "test@test.com",
                Password = "wrong"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = signin.Email,
                Name = "Test",
                PasswordHash = "HASHED"
            };

            UserRepoMock.Setup(r => r.GetUserByEmail(signin.Email)).Returns(user);
            HashProviderMock.Setup(h => h.Verify(signin.Password, "HASHED")).Returns(false);

            Assert.Throws<BadHttpRequestException>(() => AuthService.SignIn(signin));
        }

        [Test]
        public void SignIn_ShouldReturnToken_WhenPasswordCorrect()
        {
            var signin = new SignInRequestDto
            {
                Email = "test@test.com",
                Password = "123456"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = signin.Email,
                Name = "Test",
                PasswordHash = "HASHED"
            };

            UserRepoMock.Setup(r => r.GetUserByEmail(signin.Email)).Returns(user);
            HashProviderMock.Setup(h => h.Verify(signin.Password, "HASHED")).Returns(true);
            TokenProviderMock.Setup(t => t.CreateToken(user.Id, user.Email)).Returns("TOKEN");

            var (response, token) = AuthService.SignIn(signin);

            Assert.That(response.Email, Is.EqualTo(user.Email));
            Assert.That(token, Is.EqualTo("TOKEN"));
        }
    }
}