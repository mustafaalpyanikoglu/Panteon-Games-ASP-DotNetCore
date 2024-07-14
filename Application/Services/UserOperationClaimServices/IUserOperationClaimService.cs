using Domain.Concrete;

namespace Application.Services.UserService;

public interface IUserOperationClaimService
{
    public Task AssignAdminRole(int userId);
    public Task AssignGamerRole(int userId);
    public Task AssignVipGamerRole(int userId);
    public Task DeleteAdminRole(int userId);
    public Task DeleteGamerRole(int userId);
    public Task DeleteVipGamerRole(int userId);
}
