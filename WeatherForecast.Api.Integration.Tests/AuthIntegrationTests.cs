using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using WeatherForecast.Api.Dtos.Request;

namespace WeatherForecast.Api.Integration.Tests
{
    [TestFixture]
    public class AuthIntegrationTests : TestBase
    {
        [Test]
        public async Task SignUp_ShouldReturn200()
        {
            var signup = new SignUpRequestDto
            {
                Name = "YoussefTest",
                Email = $"user_{Guid.NewGuid()}@test.com",
                Password = "12345678"
            };

            var response = await Client.PostAsJsonAsync("/api/auth/signup", signup);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task SignIn_ShouldReturnAuthCookie()
        {
            var email = $"login_{Guid.NewGuid()}@test.com";

            await Client.PostAsJsonAsync("/api/auth/signup", new SignUpRequestDto
            {
                Name = "User",
                Email = email,
                Password = "12345678"
            });

            var response = await Client.PostAsJsonAsync("/api/auth/signin", new SignInRequestDto
            {
                Email = email,
                Password = "12345678"
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Headers.Contains("Set-Cookie"), Is.True);
        }

        [Test]
        public async Task SignIn_ShouldReturnError_WhenInvalid()
        {
            var response = await Client.PostAsJsonAsync("/api/auth/signin", new SignInRequestDto
            {
                Email = "missing@test.com",
                Password = "wrong123"
            });

            Assert.That((int)response.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task Logout_ShouldReturn200()
        {
            var email = $"logout_{Guid.NewGuid()}@test.com";

            await Client.PostAsJsonAsync("/api/auth/signup", new SignUpRequestDto
            {
                Name = "User",
                Email = email,
                Password = "12345678"
            });

            var loginRes = await Client.PostAsJsonAsync("/api/auth/signin", new SignInRequestDto
            {
                Email = email,
                Password = "12345678"
            });

            var cookie = loginRes.Headers.GetValues("Set-Cookie").First();
            Client.DefaultRequestHeaders.Add("Cookie", cookie);

            var response = await Client.PostAsync("/api/auth/logout", null);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}