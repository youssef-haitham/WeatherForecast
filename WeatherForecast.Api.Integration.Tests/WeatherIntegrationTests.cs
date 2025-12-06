using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using WeatherForecast.Api.Dtos.Request;

namespace WeatherForecast.Api.Integration.Tests
{
    [TestFixture]
    public class WeatherIntegrationTests : TestBase
    {
        private async Task Authenticate()
        {
            var email = $"w_{Guid.NewGuid()}@test.com";

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
        }

        [Test]
        public async Task GetWeather_ShouldReturnUnauthorized_WhenNotLoggedIn()
        {
            var response = await Client.GetAsync("/api/weather?city=Cairo");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetWeather_ShouldReturn200_WhenAuthenticated()
        {
            await Authenticate();

            var response = await Client.GetAsync("/api/weather?city=London");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}