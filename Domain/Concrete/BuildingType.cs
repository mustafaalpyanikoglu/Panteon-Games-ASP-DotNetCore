using Core.Persistence.MongoDbRepositories;
using Domain.Enums;
using MongoDB.Bson;

namespace Domain.Concrete;

// Used to store basic features and description of each building type.
public class BuildingConfig : MongoDbEntity
{
    public BuildingTypeEnum BuildingType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int BuildingCost { get; set; }
    public int ConstructionTime { get; set; }        
}

public class BuildingUpgrade : MongoDbEntity
{
    public ObjectId BuildingConfigId { get; set; }
    public int UpgradeLevel { get; set; } 
    public int UpgradeCost { get; set; }
    // generation time in seconds
    public int UpgradeTime { get; set; }     
    
    public virtual BuildingConfig? BuildingConfig { get; set; }
}


public class UserBuilding : MongoDbEntity
{
    public int UserId { get; set; }
    public ObjectId BuildingConfigId { get; set; }
    public int CurrentLevel { get; set; }
    public DateTime ConstructionStartTime { get; set; }

    public virtual BuildingConfig? BuildingConfig { get; set; }

    public UserBuilding()
    {
        
    }

    public UserBuilding(int userId, int currentLevel, DateTime constructionStartTime)
    {
        UserId = userId;
        CurrentLevel = currentLevel;
        ConstructionStartTime = constructionStartTime;
    }
}