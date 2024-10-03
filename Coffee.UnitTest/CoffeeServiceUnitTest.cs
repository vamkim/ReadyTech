using Coffee.Interface;
using Coffee.Services;

namespace Coffee.UnitTest
{
    public class CoffeeServiceUnitTest
    {
        private ICoffeeService _coffeeService;

        public CoffeeServiceUnitTest()
        {
            _coffeeService = new CoffeeService();
        }

        [Fact]
        public void BrewCoffee_ReturnsTeapotOnAprilFoolsDay()
        {
            // Arrange: Simulate April 1st
            var currentDate = new DateTime(2024, 4, 1);

            // Act
            var result = _coffeeService.BrewCoffee(currentDate);

            // Assert
            Assert.Equal(418, result.StatusCode);
        }

        [Fact]
        public void BrewCoffee_EveryFifthCall_ReturnsServiceUnavailable()
        {
            // Arrange: Call 4 times
            ResetCallCounter(); // Reset the call counter
            var currentDate = DateTime.UtcNow;
            for (int i = 0; i < 4; i++)
            {
                _coffeeService.BrewCoffee(currentDate);
            }

            // Act: Call the 5th time
            var result = _coffeeService.BrewCoffee(currentDate);

            // Assert
            Assert.Equal(503, result.StatusCode);
        }

        [Fact]
        public void BrewCoffee_ReturnsSuccessMessageAndPreparedTime()
        {
            // Arrange
            var currentDate = DateTime.UtcNow;

            // Act
            var result = _coffeeService.BrewCoffee(currentDate);

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Your piping hot coffee is ready", result.Message);
            Assert.False(string.IsNullOrWhiteSpace(result.PreparedTime)); // Prepared time should not be empty
        }

        private void ResetCallCounter()
        {
            var type = typeof(CoffeeService);
            var field = type.GetField("callCounter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            field.SetValue(null, 0); // Set the static field to 0
        }
    }
}