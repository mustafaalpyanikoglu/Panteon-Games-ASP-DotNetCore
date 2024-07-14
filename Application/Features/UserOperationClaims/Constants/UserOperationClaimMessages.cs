using Domain.Concrete;

namespace Application.Features.UserOperationClaims.Constants;

public static class UserOperationClaimMessages
{
    public const string UserOperationClaimNotFound = "user operation claim not found";
    public const string UserOperationClaimFound = "user operation claim found";
    public const string UserOperationClaimAdded = "added user operation claim";
    public const string UserOperationClaimDeleted = "deleted user operation claim";
    public const string UserOperationClaimUpdated = "updated user operation claim";
    public const string UserAdminIdShouldBeNotExists = "user admin id should be not exists";
    public const string UserGamerIdShouldBeNotExists = "user gamer id should be not exists";
    public const string UserVipGamerIdShouldBeNotExists = "user vip gamer id should be not exists";
    public const string UserMustHaveRole = "user operation claim must be available";
}
