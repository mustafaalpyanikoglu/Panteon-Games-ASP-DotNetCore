using Core.Persistence.Repositories;
using Domain.Concrete;

namespace Application.Services.Repositories;

public interface IOperationClaimRepository : IRepository<OperationClaim>, IAsyncRepository<OperationClaim> { }

