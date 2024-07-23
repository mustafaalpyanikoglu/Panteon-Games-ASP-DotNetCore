using Core.Persistence.MongoDbRepositories;
using Domain.Enums;

namespace Domain.Concrete;

// Used to store basic features and description of each building type.
public class BuildingConfig : MongoDbEntity
{
    public BuildingTypeEnum BuildingType { get; set; }
    public int BuildingCost { get; set; }
    public int ConstructionTime { get; set; }
}
