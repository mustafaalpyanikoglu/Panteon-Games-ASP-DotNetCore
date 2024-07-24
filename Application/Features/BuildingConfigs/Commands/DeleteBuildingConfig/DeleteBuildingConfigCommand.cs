using AutoMapper;
using MediatR;
using Core.Application.Pipelines.Authorization;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Services.Repositories;
using Domain.Concrete;
using Application.Features.BuildingConfigs.Dtos;
using Application.Features.BuildingConfigs.Rules;
using Core.Application.Pipelines.Caching;


namespace Application.Features.BuildingConfigs.Commands.DeleteBuildingConfig;

public class DeleteBuildingConfigCommand : IRequest<DeletedBuildingConfigDto>, ICacheRemoverRequest, ISecuredRequest
{
    public string Id { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public bool BypassCache { get; set; }
    public string CacheKey => $"GetListBuildingConfigQuery";
    public string CacheGroupKey => "GetBuildingConfig";
    public TimeSpan? SlidingExpiration { get; set; }

    string[]? ICacheRemoverRequest.CacheGroupKey => ["BuildingTypeList", "BuildingConfigById", "BuildingConfigList", "BuildingConfigListWithPagination"];

    public class DeleteBuildingConfigCommandHandler : IRequestHandler<DeleteBuildingConfigCommand, DeletedBuildingConfigDto>
    {
        private readonly IBuildingConfigRepository _buildingConfigRepository;
        private readonly IMapper _mapper;
        private readonly BuildingConfigBusinessRules _buildingConfigBusinessRules;

        public DeleteBuildingConfigCommandHandler(IBuildingConfigRepository buildingConfigDal, IMapper mapper, BuildingConfigBusinessRules buildingConfigBusinessRules)
        {
            _buildingConfigRepository = buildingConfigDal;
            _mapper = mapper;
            _buildingConfigBusinessRules = buildingConfigBusinessRules;
        }

        public async Task<DeletedBuildingConfigDto> Handle(DeleteBuildingConfigCommand request, CancellationToken cancellationToken)
        {
            await _buildingConfigBusinessRules.BuildingConfigIdShouldExistWhenSelected(request.Id);

            var mappedBuildingConfig = _mapper.Map<BuildingConfig>(request);
            var deletedBuildingConfig = await _buildingConfigRepository.DeleteAsync(mappedBuildingConfig);
            var deleteBuildingConfigDto = _mapper.Map<DeletedBuildingConfigDto>(deletedBuildingConfig);

            return deleteBuildingConfigDto;
        }
    }
}
