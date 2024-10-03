using Coffee.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeService _coffeeService;

        public CoffeeController(ICoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }

        [HttpGet("brew-coffee")]
        public IActionResult BrewCoffee()
        {
            var currentDate = DateTime.UtcNow;
            var result = _coffeeService.BrewCoffee(currentDate);

            if (result.StatusCode == 418)
            {
                return StatusCode(418); // I'm a teapot
            }

            if (result.StatusCode == 503)
            {
                return StatusCode(503); // Service Unavailable
            }

            return Ok(new
            {
                message = result.Message,
                prepared = result.PreparedTime
            });
        }
    }
}
