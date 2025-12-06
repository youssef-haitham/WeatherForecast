using NUnit.Framework;

namespace WeatherForecast.Api.Integration.Tests
{
    public abstract class TestBase
    {
        protected HttpClient Client;
        protected string BaseUrl;

        [SetUp]
        public void Setup()
        {
            BaseUrl = TestContext.Parameters.Get("ApiBaseUrl", "");

            if (string.IsNullOrWhiteSpace(BaseUrl))
                throw new Exception("ApiBaseUrl is missing from .runsettings");

            Client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }
    }
}