using Core.Application.Dtos;
using Domain.Enums;
using MongoDB.Bson;

namespace Application.Features.BuildingConfigs.Dtos;

public class BuildingConfigListDto : IDto
{
    public string Id { get; set; }
    public BuildingTypeEnum BuildingType { get; set; }
    public int BuildingCost { get; set; }
    public int ConstructionTime { get; set; }
}
