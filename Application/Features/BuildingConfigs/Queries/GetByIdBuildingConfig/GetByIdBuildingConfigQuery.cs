using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Domain.Concrete;
using MediatR;
using Application.Features.BuildingConfigs.Dtos;
using Application.Services.Repositories;
using Application.Features.BuildingConfigs.Rules;
using static Core.Security.Constants.GeneralOperationClaims;


namespace Application.Features.BuildingConfigs.Queries.GetByIdBuildingConfig;

public class GetByIdBuildingConfigQuery : IRequest<BuildingConfigDto>, ISecuredRequest
{
    public string Id { get; set; }
    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class GetByIdBuildingConfigQueryHandler : IRequestHandler<GetByIdBuildingConfigQuery, BuildingConfigDto>
    {
        private readonly IBuildingConfigRepository _buildingConfigDal;
        private readonly IMapper _mapper;
        private readonly BuildingConfigBusinessRules _buildingConfigBusinessRules;

        public GetByIdBuildingConfigQueryHandler(IBuildingConfigRepository buildingConfigDal, IMapper mapper, BuildingConfigBusinessRules buildingConfigBusinessRules)
        {
            _buildingConfigDal = buildingConfigDal;
            _mapper = mapper;
            _buildingConfigBusinessRules = buildingConfigBusinessRules;
        }

        public async Task<BuildingConfigDto> Handle(GetByIdBuildingConfigQuery request, CancellationToken cancellationToken)
        {
            await _buildingConfigBusinessRules.BuildingConfigIdShouldExistWhenSelected(request.Id);

            var buildingConfig = await _buildingConfigDal.GetAsync(m => m.Id == request.Id);
            var buildingConfigDto = _mapper.Map<BuildingConfigDto>(buildingConfig);

            return buildingConfigDto;
        }
    }
}
