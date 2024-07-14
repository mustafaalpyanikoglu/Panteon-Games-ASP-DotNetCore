using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain.Concrete;
using Persistence.Contexts;

namespace DataAccess.Concrete.EntityFramework;

public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, SqlContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(SqlContext context) : base(context)
    {
    }
}
