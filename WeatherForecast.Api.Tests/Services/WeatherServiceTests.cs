using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using WeatherForecast.Api.Entities;
using WeatherForecast.Api.Enums;
using WeatherForecast.Api.Interfaces.Repositories;
using WeatherForecast.Api.Services;

namespace WeatherForecast.Api.Tests.Services
{
    [TestFixture]
    public class WeatherServiceTests
    {
        private Mock<IWeatherRepository> WeatherRepoMock;
        private IMemoryCache MemoryCache;
        private WeatherService WeatherService;

        [SetUp]
        public void Setup()
        {
            WeatherRepoMock = new Mock<IWeatherRepository>();
            MemoryCache = new MemoryCache(new MemoryCacheOptions());
            WeatherService = new WeatherService(WeatherRepoMock.Object, MemoryCache);
        }

        [Test]
        public void GetWeather_ShouldReturnWeather_FromRepository()
        {
            var city = "Cairo";
            var weather = new Weather
            {
                City = city,
                Temperature = 25,
                Condition = WeatherCondition.Sunny
            };

            WeatherRepoMock
                .Setup(r => r.GetByCity(city))
                .Returns(weather);

            var result = WeatherService.GetWeather(city);

            Assert.That(result, Is.EqualTo(weather));
        }

        [Test]
        public void GetWeather_ShouldUseCache_OnSecondCall()
        {
            var city = "Paris";

            var weather = new Weather
            {
                City = city,
                Temperature = 15,
                Condition = WeatherCondition.Cloudy
            };

            WeatherRepoMock
                .Setup(r => r.GetByCity(city))
                .Returns(weather);

            var r1 = WeatherService.GetWeather(city);

            var r2 = WeatherService.GetWeather(city);

            Assert.That(r1, Is.EqualTo(weather));
            Assert.That(r2, Is.EqualTo(weather));

            WeatherRepoMock.Verify(r => r.GetByCity(city), Times.Once);
        }
    }
}
