using Coffee.Interface;
using Coffee.Model;

namespace Coffee.Services
{
    public class CoffeeService : ICoffeeService
    {
        private static int callCounter = 0;

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
            
            // Success response
            return new CoffeeResult
            {
                StatusCode = 200,
                Message = "Your piping hot coffee is ready",
                PreparedTime = currentDate.ToString("o") // ISO 8601 format
            };
        }
    }
}
