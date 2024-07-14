using Core.Persistence.Repositories;
using Domain.Concrete;

namespace Application.Services.Repositories;

public interface IUserRepository : IRepository<User>, IAsyncRepository<User>
{
    List<OperationClaim> GetClaims(User user);
}

