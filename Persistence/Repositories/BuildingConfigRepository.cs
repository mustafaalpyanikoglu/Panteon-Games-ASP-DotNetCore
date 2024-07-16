using Application.Services.Repositories;
using Core.Persistence.MongoDbRepositories;
using Domain.Concrete;
using Microsoft.Extensions.Options;

namespace Persistence.Repositories;

public class BuildingConfigRepository : MongoDbRepositoryBase<BuildingConfig>, IBuildingConfigRepository
{
    public BuildingConfigRepository(IOptions<MongoDbSettings> options) : base(options) { }
}
