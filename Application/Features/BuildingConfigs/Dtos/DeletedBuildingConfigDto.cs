using Core.Application.Dtos;
using MongoDB.Bson;

namespace Application.Features.BuildingConfigs.Dtos;

public class DeletedBuildingConfigDto : IDto
{
    public ObjectId Id { get; set; }
}
