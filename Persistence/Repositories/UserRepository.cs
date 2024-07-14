using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain.Concrete;
using Persistence.Contexts;

namespace DataAccess.Concrete.EntityFramework;

public class UserRepository : EfRepositoryBase<User, SqlContext>, IUserRepository
{
    public UserRepository(SqlContext context) : base(context)
    {
    }

    public List<OperationClaim> GetClaims(User user)
    {
        using (Context)
        {
            var result = from operationClaim in Context.OperationClaims
                         join userOperationClaim in Context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                         where userOperationClaim.UserId == user.Id
                         select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
            return result.ToList();
        }
    }
}
