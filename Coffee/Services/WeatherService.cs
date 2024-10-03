using Coffee.Interface;
using System.Text.Json;

namespace Coffee.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _weatherApiUrl = "https://api.openweathermap.org/data/3.0/onecall/timemachine?lat=39.099724&lon=-94.578331&dt=1643803200&appid={0}&format=json";
        private readonly string _weatherApiKey = "a90937d187a89c3489aa5eb031ebd5b2";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double> GetCurrentTemperatureAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format(_weatherApiUrl, _weatherApiKey));
            var response = await _httpClient.SendAsync(request);
            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Check if the content is not null or empty
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    // Read the JSON content only if it contains valid data
                    var weatherData = JsonSerializer.Deserialize<WeatherResponse>(content);
                    if (weatherData != null && weatherData.data.Count > 0)
                    {
                        return weatherData.data[0].temp;
                    }
                }
            }
            // Return 0 or handle it according to your application's logic
            return 0;
        }
    }

    public class WeatherResponse
    {
        public List<WeatherData> data { get; set; }
    }

    public class WeatherData
    {
        public double temp { get; set; }
    }
}
