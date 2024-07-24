using AutoMapper;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Application.Services.Repositories;
using Application.Features.BuildingConfigs.Dtos;
using static Core.Security.Constants.GeneralOperationClaims;
using Core.Application.Pipelines.Caching;


namespace Application.Features.BuildingConfigs.Queries.GetBuildingTypeList;

public class GetBuildingTypeListQuery : IRequest<List<BuildingTypeDto>>, ICachableRequest, ISecuredRequest
{
    public bool BypassCache { get; set; }
    public string CacheKey => "GetBuildingTypeListQuery";
    public string CacheGroupKey => "BuildingTypeList";
    public TimeSpan? SlidingExpiration { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class GetBuildingTypeListQueryHanlder : IRequestHandler<GetBuildingTypeListQuery, List<BuildingTypeDto>>
    {
        private readonly IBuildingConfigRepository _buildingConfigDal;
        private readonly IMapper _mapper;

        public GetBuildingTypeListQueryHanlder(IBuildingConfigRepository buildingConfigDal, IMapper mapper)
        {
            _buildingConfigDal = buildingConfigDal;
            _mapper = mapper;
        }

        public async Task<List<BuildingTypeDto>> Handle(GetBuildingTypeListQuery request, CancellationToken cancellationToken)
        {
            var buildingConfigs = _buildingConfigDal.Get();
            var mappedBuildingConfigListModel = _mapper.Map<List<BuildingTypeDto>>(buildingConfigs);
            return mappedBuildingConfigListModel.ToList();

        }
    }
}
