using NUnit.Framework;
using WeatherForecast.Api.Providers;

namespace WeatherForecast.Api.Tests.Providers
{
    [TestFixture]
    public class HashProviderTests
    {
        private HashProvider HashProvider;

        [SetUp]
        public void Setup()
        {
            HashProvider = new HashProvider();
        }

        [Test]
        public void HashPassword_ShouldReturnNonEmptyHash()
        {
            var password = "MySecret123";

            var hash = HashProvider.HashPassword(password);

            Assert.That(hash, Is.Not.Null);
            Assert.That(hash, Is.Not.Empty);
            Assert.That(hash, Is.Not.EqualTo(password));
        }

        [Test]
        public void Verify_ShouldReturnTrue_WhenPasswordMatchesHash()
        {
            var password = "MySecret123";

            var hash = HashProvider.HashPassword(password);

            var result = HashProvider.Verify(password, hash);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Verify_ShouldReturnFalse_WhenPasswordDoesNotMatchHash()
        {
            var password = "MySecret123";
            var otherPassword = "WrongPassword";

            var hash = HashProvider.HashPassword(password);

            var result = HashProvider.Verify(otherPassword, hash);

            Assert.That(result, Is.False);
        }
    }
}