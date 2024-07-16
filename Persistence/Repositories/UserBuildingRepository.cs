using Application.Services.Repositories;
using Core.Persistence.MongoDbRepositories;
using Domain.Concrete;
using Microsoft.Extensions.Options;

namespace Persistence.Repositories;

public class UserBuildingRepository : MongoDbRepositoryBase<UserBuilding>, IUserBuildingRepository
{
    public UserBuildingRepository(IOptions<MongoDbSettings> options) : base(options) { }
}