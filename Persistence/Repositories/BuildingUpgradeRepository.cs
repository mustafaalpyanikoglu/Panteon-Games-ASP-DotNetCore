using Application.Services.Repositories;
using Core.Persistence.MongoDbRepositories;
using Domain.Concrete;
using Microsoft.Extensions.Options;

namespace Persistence.Repositories;

public class BuildingUpgradeRepository : MongoDbRepositoryBase<BuildingUpgrade>, IBuildingUpgradeRepository
{
    public BuildingUpgradeRepository(IOptions<MongoDbSettings> options) : base(options) { }
}
