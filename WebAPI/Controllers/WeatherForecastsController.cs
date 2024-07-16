using Application.Services.Repositories;
using Domain.Concrete;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastsController : ControllerBase
{
    private readonly IWeatherForecastRepository _repository;

    public WeatherForecastsController(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _repository.Get();
        if (result == null)
        {
            return BadRequest("Not found");
        }

        return Ok(result.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var result = _repository.GetByIdAsync(id).Result;
        if (result == null)
        {
            return BadRequest("Not found");
        }

        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create([FromBody] WeatherForecast data)
    {
        var result = _repository.AddAsync(data).Result;
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] WeatherForecast data)
    {
        var updateDefinition = Builders<WeatherForecast>.Update
            .Set(x => x.Summary, data.Summary)
            .Set(x => x.TemperatureC, data.TemperatureC);

        var result = await _repository.UpdateAsync(id, updateDefinition);

        if (result == null)
        {
            return NotFound("WeatherForecast not found.");
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var result = _repository.DeleteAsync(id).Result;
        if (result == null)
        {
            return BadRequest("Not found");
        }

        return NoContent();
    }
}
