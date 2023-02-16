using CommonServiceLocator.Services;
using Microsoft.AspNetCore.Mvc;
using ServiceLocation;

namespace CommonServiceLocator.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly Injected<IHttpContextAccessor> _inject;

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor accessor)
    {
        _logger = logger;
        _httpContextAccessor = accessor;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public object Get()
    {
        var service = ServiceLocator.Current.GetService<IService1>();

        return new
        {
            FromService = service.GetMetrics(),
            FromControlelr = new
            {
                Direct = _httpContextAccessor.HttpContext.TraceIdentifier,
                ViaLocator = ServiceLocator.Current.GetService<IHttpContextAccessor>().HttpContext.TraceIdentifier,
                Inject = _inject.Service.HttpContext.TraceIdentifier
            }
        };
    }
}