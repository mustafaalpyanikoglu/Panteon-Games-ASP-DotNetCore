using Core.Persistence.MongoDbRepositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Concrete;

public class UserBuilding : MongoDbEntity
{
    public int UserId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    public string BuildingConfigId { get; set; }
    public DateTime ConstructionStartTime { get; set; }
    public DateTime ConstructionEndTime { get; set; }

    public virtual BuildingConfig? BuildingConfig { get; set; }
}