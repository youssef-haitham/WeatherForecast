using NUnit.Framework;
using WeatherForecast.Api.Repositories;

namespace WeatherForecast.Api.Tests.Repositories
{
    [TestFixture]
    public class WeatherRepositoryTests
    {
        private WeatherRepository WeatherRepository;

        [SetUp]
        public void Setup()
        {
            WeatherRepository = new WeatherRepository();
        }

        [Test]
        public void GetWeather_ShouldReturnWeatherObject()
        {
            var result = WeatherRepository.GetByCity("Cairo");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.City, Is.EqualTo("Cairo"));
        }

        [Test]
        public void GetWeather_ShouldReturnTemperatureWithinRange()
        {
            var result = WeatherRepository.GetByCity("London");

            Assert.That(result.Temperature, Is.InRange(-10, 50));
        }

        [Test]
        public void GetWeather_ShouldReturnValidWeatherCondition()
        {
            var result = WeatherRepository.GetByCity("Paris");

            Assert.That(Enum.IsDefined(result.Condition), Is.True);
        }
    }
}