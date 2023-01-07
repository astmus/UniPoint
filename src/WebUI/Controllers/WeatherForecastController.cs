using Microsoft.AspNetCore.Mvc;
using MissBot.WeatherForecasts.Queries.GetWeatherForecasts;

namespace WebUI.Controllers;
public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
