using Core.Application.Dtos;
using Domain.Enums;
using MongoDB.Bson;

namespace Application.Features.BuildingConfigs.Dtos;

public class UpdatedBuildingConfigDto : IDto
{
    public ObjectId Id { get; set; }
    public BuildingTypeEnum BuildingType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int BuildingCost { get; set; }
    public int ConstructionTime { get; set; }
}
