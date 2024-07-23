using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Requests;
using MediatR;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Services.Repositories;
using Application.Features.BuildingConfigs.Dtos;
using Domain.Constants;

namespace Application.Features.BuildingConfigs.Queries.GetListBuildingConfig;

public class GetListBuildingConfigQuery : IRequest<List<BuildingConfigListDto>>/*, ICachableRequest*/, ISecuredRequest
{
    //public bool BypassCache { get; set; }
    //public string CacheKey => $"GetListBuildingConfigQuery";
    //public string CacheGroupKey => "GetBuildingConfig";
    //public TimeSpan? SlidingExpiration { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class GetListBuildingConfigQueryHanlder : IRequestHandler<GetListBuildingConfigQuery, List<BuildingConfigListDto>>
    {
        private readonly IBuildingConfigRepository _buildingConfigDal;
        private readonly IMapper _mapper;

        public GetListBuildingConfigQueryHanlder(IBuildingConfigRepository buildingConfigDal, IMapper mapper)
        {
            _buildingConfigDal = buildingConfigDal;
            _mapper = mapper;
        }

        public async Task<List<BuildingConfigListDto>> Handle(GetListBuildingConfigQuery request, CancellationToken cancellationToken)
        {
            var buildingConfigs = _buildingConfigDal.Get();
            var mappedBuildingConfigListModel = _mapper.Map<List<BuildingConfigListDto>>(buildingConfigs);
            return mappedBuildingConfigListModel.ToList();

        }
    }
}
