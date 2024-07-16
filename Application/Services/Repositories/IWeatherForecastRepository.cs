using Core.Persistence.MongoDbRepositories;
using Domain.Concrete;
using MongoDB.Bson;

namespace Application.Services.Repositories;

public interface IWeatherForecastRepository : IMongoDbRepository<WeatherForecast, string> { }