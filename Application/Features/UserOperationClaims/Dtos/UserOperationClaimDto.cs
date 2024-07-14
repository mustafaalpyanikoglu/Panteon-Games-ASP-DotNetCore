using Core.Application.Dtos;

namespace Application.Features.UserOperationClaims.Dtos;

public class UserOperationClaimDto : IDto
{
    public string UserName { get; set; }
    public string OperationClaimName { get; set; }
    public string OperationClaimNameDescription { get; set; }
}
