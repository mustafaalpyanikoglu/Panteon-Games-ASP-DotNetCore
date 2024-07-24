using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions;
using Application.Services.Repositories;
using Application.Features.BuildingConfigs.Constants;
using Domain.Enums;

namespace Application.Features.BuildingConfigs.Rules;

public class BuildingConfigBusinessRules : BaseBusinessRules
{
    private readonly IBuildingConfigRepository _buildingConfigRepository;

    public BuildingConfigBusinessRules(IBuildingConfigRepository buildingConfigRepository)
    {
        _buildingConfigRepository = buildingConfigRepository;
    }

    public async Task BuildingConfigIdShouldExistWhenSelected(string? id)
    {
        if (id == null) throw new BusinessException(BuildingConfigMessages.BuildingConfigNotFound);
        var result = await _buildingConfigRepository.GetByIdAsync(id);
        if (result == null) throw new BusinessException(BuildingConfigMessages.BuildingConfigNotFound);
    }

    public async Task EnsureBuildingTypeNotVisibleInCombobox(BuildingTypeEnum buildingType)
    {
        var existingBuildingConfig = await _buildingConfigRepository.GetAsync(x => x.BuildingType == buildingType);
        if (existingBuildingConfig != null) throw new BusinessException(BuildingConfigMessages.BuildingTypeAlreadyAdded);
    }

}
