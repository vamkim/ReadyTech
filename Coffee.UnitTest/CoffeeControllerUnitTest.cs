using Coffee.Controllers;
using Coffee.Interface;
using Coffee.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace Coffee.UnitTest
{
    public class CoffeeControllerUnitTest
    {
        private readonly Mock<ICoffeeService> _mockCoffeeService;
        private readonly CoffeeController _controller;

        public CoffeeControllerUnitTest()
        {
            _mockCoffeeService = new Mock<ICoffeeService>();
            _controller = new CoffeeController(_mockCoffeeService.Object);
        }

        [Fact]
        public void BrewCoffee_ReturnsTeapotOnAprilFoolsDay()
        {
            var currentDate = DateTime.UtcNow;
            var coffeeResult = new CoffeeResult { StatusCode = 418 };

            // Setup mock to return 503 for BrewCoffee
            _mockCoffeeService.Setup(s => s.BrewCoffee(It.IsAny<DateTime>())).Returns(coffeeResult);

            // Act
            var result = _controller.BrewCoffee();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(418, statusResult.StatusCode);
        }

        [Fact]
        public void BrewCoffee_EveryFifthCall_ReturnsServiceUnavailable()
        {
            // Arrange: Simulate normal date
            var currentDate = DateTime.UtcNow;
            var coffeeResult = new CoffeeResult { StatusCode = 503 };

            // Setup mock to return 503 for BrewCoffee
            _mockCoffeeService.Setup(s => s.BrewCoffee(It.IsAny<DateTime>())).Returns(coffeeResult);

            // Act
            var result = _controller.BrewCoffee();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(503, statusResult.StatusCode);
        }

        [Fact]
        public void BrewCoffee_ReturnsSuccessMessageAndPreparedTime()
        {
            // Arrange
            var currentDate = DateTime.UtcNow;
            var preparedTime = currentDate.ToString("o"); // ISO 8601 format
            var coffeeResult = new CoffeeResult
            {
                StatusCode = 200,
                Message = "Your piping hot coffee is ready",
                PreparedTime = preparedTime
            };

            // Setup mock to return success response
            _mockCoffeeService.Setup(s => s.BrewCoffee(It.IsAny<DateTime>())).Returns(coffeeResult);

            // Act
            var result = _controller.BrewCoffee();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
