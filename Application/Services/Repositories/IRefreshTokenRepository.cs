using Core.Persistence.Repositories;
using Domain.Concrete;

namespace Application.Services.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>, IAsyncRepository<RefreshToken> { }

