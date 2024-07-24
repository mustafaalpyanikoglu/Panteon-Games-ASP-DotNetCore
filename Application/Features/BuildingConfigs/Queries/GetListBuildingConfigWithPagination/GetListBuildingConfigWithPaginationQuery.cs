using AutoMapper;
using Core.Application.Pipelines.Authorization;
using MediatR;
using Application.Services.Repositories;
using static Core.Security.Constants.GeneralOperationClaims;
using Core.Application.Requests;
using Application.Features.UserOperationClaims.Models;
using Core.Application.Pipelines.Caching;

namespace Application.Features.BuildingConfigs.Queries.GetListBuildingConfigWithPagination;

public class GetListBuildingConfigWithPaginationQuery : IRequest<BuildingConfigListModel>, ICachableRequest, ISecuredRequest
{
    public bool BypassCache { get; set; }
    public string CacheKey => $"GetListBuildingConfigWithPaginationQuery_Page_{PageRequest.Page}_Size_{PageRequest.PageSize}";
    public string CacheGroupKey => "BuildingConfigListWithPagination";
    public TimeSpan? SlidingExpiration { get; set; }

    public PageRequest PageRequest { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class GetListBuildingConfigWithPaginationQueryHanlder : IRequestHandler<GetListBuildingConfigWithPaginationQuery, BuildingConfigListModel>
    {
        private readonly IBuildingConfigRepository _buildingConfigDal;
        private readonly IMapper _mapper;

        public GetListBuildingConfigWithPaginationQueryHanlder(IBuildingConfigRepository buildingConfigDal, IMapper mapper)
        {
            _buildingConfigDal = buildingConfigDal;
            _mapper = mapper;
        }

        public async Task<BuildingConfigListModel> Handle(GetListBuildingConfigWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var buildingConfigs = await _buildingConfigDal.GetListAsync
                (
                    index: request.PageRequest.Page,
                    size: request.PageRequest.PageSize
                );
            var mappedBuildingConfigListModel = _mapper.Map<BuildingConfigListModel>(buildingConfigs);
            return mappedBuildingConfigListModel;

        }
    }
}
