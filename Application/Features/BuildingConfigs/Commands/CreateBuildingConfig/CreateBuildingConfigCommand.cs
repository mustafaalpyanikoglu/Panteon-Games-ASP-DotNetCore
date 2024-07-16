using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Domain.Concrete;
using MediatR;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.BuildingConfigs.Dtos;
using Application.Features.BuildingConfigs.Rules;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Application.Services.ImageServices;
using NArchitecture.Core.Application.Pipelines.Caching;

namespace Application.Features.BuildingConfigs.Commands.CreateBuildingConfig
{
    public class CreateBuildingConfigCommand : IRequest<CreatedBuildingConfigDto>, ICacheRemoverRequest/*, ISecuredRequest*/
    {
        
        public bool BypassCache { get; set; }
        public string CacheKey => $"GetListBuildingConfigQuery";
        public string CacheGroupKey => "GetBuildingConfig";
        public TimeSpan? SlidingExpiration { get; set; }
        string[]? ICacheRemoverRequest.CacheGroupKey => ["GetBuildingConfig"];


        public BuildingTypeEnum BuildingType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public int BuildingCost { get; set; }
        public int ConstructionTime { get; set; }


        //public string[] Roles => new[] { ADMIN, GAMER, VIP };

        public class CreateBuildingConfigCommandHandler(
            IBuildingConfigRepository buildingConfigDal, 
            IMapper mapper,
            ImageServiceBase imageService,
            BuildingConfigBusinessRules buildingConfigBusinessRules)
            : IRequestHandler<CreateBuildingConfigCommand, CreatedBuildingConfigDto>
        {
            private readonly IBuildingConfigRepository _buildingConfigRepository = buildingConfigDal;
            private readonly IMapper _mapper = mapper;
            private readonly ImageServiceBase _imageService = imageService;
            private readonly BuildingConfigBusinessRules _buildingConfigBusinessRules = buildingConfigBusinessRules;

            public async Task<CreatedBuildingConfigDto> Handle(CreateBuildingConfigCommand request, CancellationToken cancellationToken)
            {
                var mappedBuildingConfig = _mapper.Map<BuildingConfig>(request);

                var imageUrl = await _imageService.UploadAsync(request.File);
                mappedBuildingConfig.ImageUrl = imageUrl;

                var createdBuildingConfig = await _buildingConfigRepository.AddAsync(mappedBuildingConfig);
                var createBuildingConfigDto = _mapper.Map<CreatedBuildingConfigDto>(createdBuildingConfig);


                return createBuildingConfigDto;
            }
        }
    }
}
