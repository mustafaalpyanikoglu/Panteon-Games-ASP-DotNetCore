using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain.Concrete;
using Persistence.Contexts;

namespace DataAccess.Concrete.EntityFramework;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, SqlContext>, IUserOperationClaimRepository
{
    public UserOperationClaimRepository(SqlContext context) : base(context)
    {
    }
}
