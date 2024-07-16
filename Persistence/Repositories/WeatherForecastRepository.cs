using Application.Services.Repositories;
using Core.Persistence.MongoDbRepositories;
using Domain.Concrete;
using Microsoft.Extensions.Options;

namespace Persistence.Repositories;

public class WeatherForecastRepository : MongoDbRepositoryBase<WeatherForecast>, IWeatherForecastRepository
{
    public WeatherForecastRepository(IOptions<MongoDbSettings> options) : base(options) { }
}