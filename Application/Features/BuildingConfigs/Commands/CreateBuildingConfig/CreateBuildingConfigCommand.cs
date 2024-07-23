using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Domain.Concrete;
using MediatR;
using Application.Features.BuildingConfigs.Dtos;
using Application.Features.BuildingConfigs.Rules;
using Domain.Enums;
using Core.Application.Pipelines.Caching;
using static Core.Security.Constants.GeneralOperationClaims;

namespace Application.Features.BuildingConfigs.Commands.CreateBuildingConfig;

public class CreateBuildingConfigCommand : IRequest<CreatedBuildingConfigDto>/*, ICacheRemoverRequest*/, ISecuredRequest
{
    public BuildingTypeEnum BuildingType { get; set; }
    public int BuildingCost { get; set; }
    public int ConstructionTime { get; set; }

    //public bool BypassCache { get; set; }
    //public string CacheKey => $"GetListBuildingConfigQuery";
    //public string CacheGroupKey => "GetBuildingConfig";
    //public TimeSpan? SlidingExpiration { get; set; }

    //string[]? ICacheRemoverRequest.CacheGroupKey => ["GetBuildingConfig"];


    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class CreateBuildingConfigCommandHandler(
        IBuildingConfigRepository buildingConfigDal, 
        IMapper mapper,
        BuildingConfigBusinessRules buildingConfigBusinessRules)
        : IRequestHandler<CreateBuildingConfigCommand, CreatedBuildingConfigDto>
    {
        private readonly IBuildingConfigRepository _buildingConfigRepository = buildingConfigDal;
        private readonly IMapper _mapper = mapper;
        private readonly BuildingConfigBusinessRules _buildingConfigBusinessRules = buildingConfigBusinessRules;

        public async Task<CreatedBuildingConfigDto> Handle(CreateBuildingConfigCommand request, CancellationToken cancellationToken)
        {
            var mappedBuildingConfig = _mapper.Map<BuildingConfig>(request);
            var createdBuildingConfig = await _buildingConfigRepository.AddAsync(mappedBuildingConfig);
            var createBuildingConfigDto = _mapper.Map<CreatedBuildingConfigDto>(createdBuildingConfig);
            return createBuildingConfigDto;
        }
    }
}
