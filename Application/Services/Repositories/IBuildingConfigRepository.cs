using Core.Persistence.MongoDbRepositories;
using Domain.Concrete;

namespace Application.Services.Repositories;

public interface IBuildingConfigRepository : IMongoDbRepository<BuildingConfig, string> { }
