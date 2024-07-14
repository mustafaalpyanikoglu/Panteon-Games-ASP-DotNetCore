using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions;
using Domain.Concrete;
using static Application.Features.OperationClaims.Constants.OperationClaimMessages;
using static Application.Features.UserOperationClaims.Constants.UserOperationClaimMessages;

namespace Application.Features.UserOperationClaims.Rules;

public class UserOperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IUserOperationClaimRepository _userOperationClaimDal;

    public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimDal)
    {
        _userOperationClaimDal = userOperationClaimDal;
    }

    public Task UserOperationClaimMustBeAvailable(UserOperationClaim? userOperationClaim)
    {
        if (userOperationClaim == null) throw new BusinessException(UserMustHaveRole);
        return Task.CompletedTask;
    }

    public async Task UserOperationClaimMustBeAvailable()
    {
        List<UserOperationClaim>? results = _userOperationClaimDal.GetAll();
        if (results.Count <= 0) throw new BusinessException(UserOperationClaimNotFound);
    }

    public async Task UserOperationClaimIdMustBeAvailable(int userOperationClaimId)
    {
        UserOperationClaim? result = await _userOperationClaimDal.GetAsync(t => t.Id == userOperationClaimId);
        if (result == null) throw new BusinessException(UserOperationClaimNotFound);
    }
    
    public Task UserShouldNotHaveAdminRole(UserOperationClaim? userOperationClaim)
    {
        if(userOperationClaim != null) throw new BusinessException(UserAdminIdShouldBeNotExists);
        return Task.CompletedTask;
    }

    public Task UserShouldNotHaveGamerRole(UserOperationClaim? userOperationClaim)
    {
        if (userOperationClaim != null) throw new BusinessException(UserGamerIdShouldBeNotExists);
        return Task.CompletedTask;
    }

    public Task UserShouldNotHaveVipGamerRole(UserOperationClaim? userOperationClaim)
    {
        if (userOperationClaim != null) throw new BusinessException(UserVipGamerIdShouldBeNotExists);
        return Task.CompletedTask;
    }
}
