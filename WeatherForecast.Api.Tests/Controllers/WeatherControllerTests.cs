using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WeatherForecast.Api.Controllers;
using WeatherForecast.Api.Entities;
using WeatherForecast.Api.Enums;
using WeatherForecast.Api.Interfaces.Services;

namespace WeatherForecast.Api.Tests.Controllers
{
    [TestFixture]
    public class WeatherControllerTests
    {
        private Mock<IWeatherService> WeatherServiceMock;
        private WeatherController WeatherController;

        [SetUp]
        public void Setup()
        {
            WeatherServiceMock = new Mock<IWeatherService>();

            WeatherController = new WeatherController(WeatherServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public void GetWeather_ShouldReturnBadRequest_WhenCityIsEmpty()
        {
            var result = WeatherController.GetWeather("");

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void GetWeather_ShouldReturnBadRequest_WhenCityIsNull()
        {
            var result = WeatherController.GetWeather(null);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void GetWeather_ShouldReturnOk_WhenCityIsValid()
        {
            var city = "Cairo";

            var weather = new Weather
            {
                City = city,
                Temperature = 27,
                Condition = WeatherCondition.Sunny
            };

            WeatherServiceMock
                .Setup(s => s.GetWeather(city))
                .Returns(weather);

            var result = WeatherController.GetWeather(city);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var ok = result as OkObjectResult;
            Assert.That(ok!.Value, Is.EqualTo(weather));
        }

        [Test]
        public void GetWeather_ShouldCallServiceOnce()
        {
            var city = "London";
            var weather = new Weather
            {
                City = city,
                Temperature = 18,
                Condition = WeatherCondition.Cloudy
            };

            WeatherServiceMock
                .Setup(s => s.GetWeather(city))
                .Returns(weather);

            WeatherController.GetWeather(city);

            WeatherServiceMock.Verify(s => s.GetWeather(city), Times.Once);
        }
    }
}