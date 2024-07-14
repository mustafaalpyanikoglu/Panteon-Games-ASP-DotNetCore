using Application.Features.UserOperationClaims.Rules;
using Application.Services.Repositories;
using static Core.Security.Constants.GeneralOperationClaimsId;
using static Application.Features.UserOperationClaims.Constants.UserOperationClaimMessages;
using Domain.Concrete;

namespace Application.Services.UserService;

public class UserOperationClaimService
    (
        IUserOperationClaimRepository userOperationClaimRepository, 
        UserOperationClaimBusinessRules userOperationClaimBusinessRules
    ) : IUserOperationClaimService
{
    private readonly IUserOperationClaimRepository _userOperationClaimRepository = userOperationClaimRepository;
    private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules = userOperationClaimBusinessRules;

    public async Task AssignAdminRole(int userId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == userId && x.OperationClaimId == ADMIN_ID);
        await _userOperationClaimBusinessRules.UserShouldNotHaveAdminRole(userOperationClaim);

        var createUserOperationClaim = new UserOperationClaim()
        {
            OperationClaimId = ADMIN_ID,
            UserId = userId
        };

        await _userOperationClaimRepository.AddAsync(createUserOperationClaim);
    }

    public async Task AssignGamerRole(int userId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == userId && x.OperationClaimId == GAMER_ID);
        await _userOperationClaimBusinessRules.UserShouldNotHaveGamerRole(userOperationClaim);

        var createUserOperationClaim = new UserOperationClaim()
        {
            OperationClaimId = GAMER_ID,
            UserId = userId
        };

        await _userOperationClaimRepository.AddAsync(createUserOperationClaim);
    }

    public async Task AssignVipGamerRole(int userId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == userId && x.OperationClaimId == VIP_GAMER_ID);
        await _userOperationClaimBusinessRules.UserShouldNotHaveVipGamerRole(userOperationClaim);

        var createUserOperationClaim = new UserOperationClaim()
        {
            OperationClaimId = VIP_GAMER_ID,
            UserId = userId
        };

        await _userOperationClaimRepository.AddAsync(createUserOperationClaim);
    }

    public async Task DeleteAdminRole(int userId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == userId && x.OperationClaimId == ADMIN_ID);
        await _userOperationClaimBusinessRules.UserOperationClaimMustBeAvailable(userOperationClaim);

        await _userOperationClaimRepository.DeleteAsync(userOperationClaim!);
    }

    public async Task DeleteGamerRole(int userId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == userId && x.OperationClaimId == GAMER_ID);
        await _userOperationClaimBusinessRules.UserOperationClaimMustBeAvailable(userOperationClaim);

        await _userOperationClaimRepository.DeleteAsync(userOperationClaim!);
    }

    public async Task DeleteVipGamerRole(int userId)
    {
        var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == userId && x.OperationClaimId == VIP_GAMER_ID);
        await _userOperationClaimBusinessRules.UserOperationClaimMustBeAvailable(userOperationClaim);

        await _userOperationClaimRepository.DeleteAsync(userOperationClaim!);
    }
}
