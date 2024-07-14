using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain.Concrete;
using Persistence.Contexts;

namespace DataAccess.Concrete.EntityFramework;

public class OperationClaimRepository : EfRepositoryBase<OperationClaim, SqlContext>, IOperationClaimRepository
{
    public OperationClaimRepository(SqlContext context) : base(context)
    {
    }
}
