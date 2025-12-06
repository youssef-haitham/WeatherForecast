using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WeatherForecast.Api.Providers;
using WeatherForecast.Api.Settings;

namespace WeatherForecast.Api.Tests.Providers
{
    [TestFixture]
    public class TokenProviderTests
    {
        private TokenProvider TokenProvider;

        [SetUp]
        public void Setup()
        {
            var jwtSettings = new JwtSettings
            {
                Key = Convert.ToBase64String(Encoding.UTF8.GetBytes("super_secret_key_for_tests_123456")),
                Issuer = "WeatherApi",
                Audience = "WeatherApiUsers",
                ExpiresInHours = 1
            };

            var optionsMock = new Mock<IOptions<JwtSettings>>();
            optionsMock.Setup(o => o.Value).Returns(jwtSettings);

            TokenProvider = new TokenProvider(optionsMock.Object);
        }

        [Test]
        public void CreateToken_ShouldReturn_NonEmptyString()
        {
            var userId = Guid.NewGuid();
            var email = "test@test.com";

            var token = TokenProvider.CreateToken(userId, email);

            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);
        }

        [Test]
        public void CreateToken_ShouldContainUserIdAndEmailClaims()
        {
            var userId = Guid.NewGuid();
            var email = "test@test.com";

            var token = TokenProvider.CreateToken(userId, email);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == "id");
            var emailClaim = jwt.Claims.FirstOrDefault(c => c.Type == "email");

            Assert.That(idClaim, Is.Not.Null);
            Assert.That(emailClaim, Is.Not.Null);

            Assert.That(idClaim!.Value, Is.EqualTo(userId.ToString()));
            Assert.That(emailClaim!.Value, Is.EqualTo(email));
        }
    }
}