using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WeatherForecast.Api.Controllers;
using WeatherForecast.Api.Dtos.Request;
using WeatherForecast.Api.Dtos.Response;
using WeatherForecast.Api.Interfaces.Services;

namespace WeatherForecast.Api.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> AuthServiceMock;
        private AuthController AuthController;

        [SetUp]
        public void Setup()
        {
            AuthServiceMock = new Mock<IAuthService>();

            AuthController = new AuthController(AuthServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public void SignUp_ShouldReturnOk_WhenSuccess()
        {
            var request = new SignUpRequestDto
            {
                Name = "Test",
                Email = "test@test.com",
                Password = "123456"
            };

            var authResponse = new AuthResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = "test@test.com"
            };

            AuthServiceMock
                .Setup(s => s.SignUp(request))
                .Returns((authResponse, "jwt-token"));

            var result = AuthController.SignUp(request) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Value, Is.EqualTo(authResponse));

            var cookieHeader = AuthController.Response.Headers.SetCookie;
            Assert.That(cookieHeader.Count, Is.GreaterThan(0));
        }

        [Test]
        public void SignIn_ShouldReturnOk_WhenSuccess()
        {
            var request = new SignInRequestDto
            {
                Email = "test@test.com",
                Password = "123456"
            };

            var authResponse = new AuthResponseDto
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = "test@test.com"
            };

            AuthServiceMock
                .Setup(s => s.SignIn(request))
                .Returns((authResponse, "jwt-token"));

            var result = AuthController.SignIn(request) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Value, Is.EqualTo(authResponse));

            var cookieHeader = AuthController.Response.Headers.SetCookie;
            Assert.That(cookieHeader.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Logout_ShouldReturnOk_AndDeleteCookie()
        {
            var result = AuthController.Logout();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var cookieHeader = AuthController.Response.Headers.SetCookie;
            Assert.That(cookieHeader.Count, Is.GreaterThan(0));
        }
    }
}