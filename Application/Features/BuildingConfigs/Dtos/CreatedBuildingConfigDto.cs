using Core.Application.Dtos;
using Domain.Enums;

namespace Application.Features.BuildingConfigs.Dtos;

public class CreatedBuildingConfigDto : IDto
{
    public BuildingTypeEnum BuildingType { get; set; }
    public int BuildingCost { get; set; }
    public int ConstructionTime { get; set; }
}
