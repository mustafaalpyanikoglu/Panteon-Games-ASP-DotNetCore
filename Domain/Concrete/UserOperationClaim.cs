using Core.Persistence.Repositories;

namespace Domain.Concrete;

public class UserOperationClaim : BaseEntity
{
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }

    public virtual User User { get; set; }
    public virtual OperationClaim OperationClaim { get; set; }

    public UserOperationClaim()
    {

    }

    public UserOperationClaim(int id, int userId, int operationClaimId) : this()
    {
        Id = id;
        UserId = userId;
        OperationClaimId = operationClaimId;
    }
}
