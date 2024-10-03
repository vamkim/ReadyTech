using Coffee.Interface;
using Coffee.Model;

namespace Coffee.Services
{
    public class CoffeeService : ICoffeeService
    {
        private static int callCounter = 0;
        private readonly IWeatherService _weatherService;

        public CoffeeService(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public CoffeeResult BrewCoffee(DateTime currentDate)
        {
            // Check if it's April 1st
            if (currentDate.Month == 4 && currentDate.Day == 1)
            {
                return new CoffeeResult { StatusCode = 418 };
            }

            // Increment the call counter
            callCounter++;

            // Every 5th call returns 503 Service Unavailable
            if (callCounter % 5 == 0)
            {
                return new CoffeeResult { StatusCode = 503 };
            }

            // Fetch the current temperature
            var temperatureTask = _weatherService.GetCurrentTemperatureAsync();
            temperatureTask.Wait();
            var temperature = temperatureTask.Result;

            // Prepare the message based on the temperature
            string message = temperature > 30 ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready";

            // Success response
            return new CoffeeResult
            {
                StatusCode = 200,
                Message = message,
                PreparedTime = currentDate.ToString("o") // ISO 8601 format
            };
        }
    }
}
