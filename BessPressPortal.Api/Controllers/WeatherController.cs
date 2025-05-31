using BessPressPortal.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BessPressPortal.Api.Controllers
{
    // Attributes to make this a Web API controller
    [ApiController] // Indicates that this class is an API controller
    [Route("[controller]")] // Defines the base route for this controller (e.g., /weather)
    public class WeatherController : ControllerBase // Inherit from ControllerBase for API controllers
    {
        // Move the summaries array here
        private static readonly string[] summaries = new[]
        {
            "0Freezing", "1Bracing", "realyChilly", "Coold", "Mildly", "Warmish", "Balmy", "HothotHot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherController> _logger; // Inject a logger (optional but good practice)

        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        // Define the HTTP GET endpoint
        [HttpGet] // This method will respond to GET requests to /weather (or /weatherforecast if you change the route)
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Fetching weather forecast."); // Log when this endpoint is hit

            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();

            return forecast;
        }

        // You can add more endpoints here, e.g.,
        // [HttpGet("{id}")]
        // public ActionResult<WeatherForecast> GetById(int id) { /* ... */ }
    }

}
