namespace HotelApp.WebApi.Controllers
{
    using HotelApp.Data;
    using HotelApp.Services.Core;
    using HotelApp.Services.Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HotelAppDbContext dbContext;
        private readonly IBookingService bookingService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, HotelAppDbContext dbContext, IBookingService bookingService)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.bookingService = bookingService;
        }

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
