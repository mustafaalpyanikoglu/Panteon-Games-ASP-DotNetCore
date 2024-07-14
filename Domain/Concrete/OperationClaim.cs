using Core.Persistence.Repositories;

namespace Domain.Concrete;

public class OperationClaim : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public OperationClaim()
    {

    }

    public OperationClaim(int id, string name, string description) : this()
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
