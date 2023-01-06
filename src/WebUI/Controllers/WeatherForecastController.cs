using Microsoft.AspNetCore.Mvc;
using MissBot.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace MissBot.WebUI.Controllers;
public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
