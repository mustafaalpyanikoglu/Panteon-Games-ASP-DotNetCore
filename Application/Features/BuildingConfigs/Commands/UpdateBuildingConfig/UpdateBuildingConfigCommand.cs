using AutoMapper;
using Application.Services.Repositories;
using MediatR;
using Core.Application.Pipelines.Authorization;
using static Core.Security.Constants.GeneralOperationClaims;
using Domain.Concrete;
using Application.Features.BuildingConfigs.Dtos;
using Application.Features.BuildingConfigs.Rules;
using MongoDB.Driver;
using Domain.Enums;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using Application.Services.ImageServices;

namespace Application.Features.BuildingConfigs.Commands;

public class UpdateBuildingConfigCommand : IRequest<UpdatedBuildingConfigDto>/*, ISecuredRequest*/
{
    public string Id { get; set; }
    public BuildingTypeEnum BuildingType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IFormFile? File { get; set; }
    public int BuildingCost { get; set; }
    public int ConstructionTime { get; set; }

    //public string[] Roles => new[] {ADMIN, GAMER, VIP };

    public class UpdateBuildingConfigCommandHandler(
        IBuildingConfigRepository buildingConfigDal, 
        IMapper mapper,
        ImageServiceBase imageService,
        BuildingConfigBusinessRules buildingConfigBusinessRules) 
        : IRequestHandler<UpdateBuildingConfigCommand, UpdatedBuildingConfigDto>
    {
        private readonly IBuildingConfigRepository _buildingConfigDal = buildingConfigDal;
        private readonly IMapper _mapper = mapper;
        private readonly ImageServiceBase _imageService = imageService;
        private readonly BuildingConfigBusinessRules _buildingConfigBusinessRules = buildingConfigBusinessRules;

        public async Task<UpdatedBuildingConfigDto> Handle(UpdateBuildingConfigCommand request, CancellationToken cancellationToken)
        {
            await _buildingConfigBusinessRules.BuildingConfigIdShouldExistWhenSelected(request.Id);

            var mappedBuildingConfig = _mapper.Map<BuildingConfig>(request);

            var updateDefinition = Builders<BuildingConfig>
                .Update
                .Set(x => x.Name, request.Name)
                .Set(x => x.BuildingType, request.BuildingType)
                .Set(x => x.Description, request.Description)
                .Set(x => x.BuildingCost, request.BuildingCost)
                .Set(x => x.ConstructionTime, request.ConstructionTime);

            string imageUrl = string.Empty;

            if(request.File != null)
            {
                imageUrl = await _imageService.UpdateAsync(request.File, request.ImageUrl);
                updateDefinition.Set(x => x.ImageUrl, imageUrl);
            }

            var updatedBuildingConfig = await _buildingConfigDal.UpdateAsync(request.Id, updateDefinition);
            var updateBuildingConfigDto = _mapper.Map<UpdatedBuildingConfigDto>(updatedBuildingConfig);

            return updateBuildingConfigDto;
        }
    }
}
