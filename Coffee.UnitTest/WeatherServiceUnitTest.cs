using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Coffee.Interface;
using Moq;
using Moq.Protected;
using Xunit;

namespace Coffee.Services.Tests
{
    public class WeatherServiceUnitTest
    {

        public WeatherServiceUnitTest()
        {
         
        }
        [Fact]
        public async Task GetCurrentTemperatureAsync_ReturnsZero_WhenResponseContentIsEmpty()
        {
            // Arrange
            var handler = new TestHttpMessageHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty), // Simulate an empty response
            });
            var httpClient = new HttpClient(handler);
            var _weatherService = new WeatherService(httpClient);
            // Act
            var result = await _weatherService.GetCurrentTemperatureAsync();

            // Assert
            Assert.Equal(0, result); // Should return 0 for empty content
        }

        [Fact]
        public async Task GetCurrentTemperatureAsync_ReturnsTemperature_WhenResponseIsSuccessful()
        {
            // Arrange
            var expectedTemperature = 25.0; // Example temperature
            var weatherResponse = new WeatherResponse
            {
                data = new List<WeatherData>
                    {
                        new WeatherData { temp = expectedTemperature }
                    }
            };

            var handler = new TestHttpMessageHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(weatherResponse),
            });

            var httpClient = new HttpClient(handler);
            var _weatherService = new WeatherService(httpClient);
            // Act
            var result = await _weatherService.GetCurrentTemperatureAsync();

            // Assert
            Assert.Equal(expectedTemperature, result);
        }

        //[Fact]
        //public async Task GetCurrentTemperatureAsync_ReturnsZero_WhenResponseIsNotSuccessful()
        //{
        //    // Arrange
        //    var handler = new TestHttpMessageHandler(new HttpResponseMessage
        //    {
        //        StatusCode = HttpStatusCode.BadRequest,
        //    });
        //    var httpClient = new HttpClient(handler);

        //    // Act
        //    var result = await _weatherService.GetCurrentTemperatureAsync();

        //    // Assert
        //    Assert.Equal(0, result);
        //}

        //[Fact]
        //public async Task GetCurrentTemperatureAsync_ReturnsZero_WhenNoDataReturned()
        //{
        //    // Arrange
        //    var weatherResponse = new WeatherResponse
        //    {
        //        Data = new List<WeatherData>() // No data
        //    };

        //    var handler = new TestHttpMessageHandler(new HttpResponseMessage
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Content = JsonContent.Create(weatherResponse),
        //    });
        //    var httpClient = new HttpClient(handler);

        //    // Act
        //    var result = await _weatherService.GetCurrentTemperatureAsync();

        //    // Assert
        //    Assert.Equal(0, result);
        //}
    }

    public class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _responseMessage;

        public TestHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseMessage);
        }
    }
}
