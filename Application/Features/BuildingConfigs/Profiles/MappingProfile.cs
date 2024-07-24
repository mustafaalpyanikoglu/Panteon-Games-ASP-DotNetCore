using Application.Features.BuildingConfigs.Commands;
using Application.Features.BuildingConfigs.Commands.CreateBuildingConfig;
using Application.Features.BuildingConfigs.Commands.DeleteBuildingConfig;
using Application.Features.BuildingConfigs.Dtos;
using Application.Features.UserOperationClaims.Models;
using AutoMapper;
using Core.Persistence.Paging;
using Domain.Concrete;

namespace Business.Features.BuildingConfigs.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BuildingConfig, CreateBuildingConfigCommand>().ReverseMap();
        CreateMap<BuildingConfig, DeleteBuildingConfigCommand>().ReverseMap();
        CreateMap<BuildingConfig, UpdateBuildingConfigCommand>().ReverseMap();
        CreateMap<BuildingConfig, CreatedBuildingConfigDto>().ReverseMap();
        CreateMap<BuildingConfig, DeletedBuildingConfigDto>().ReverseMap();
        CreateMap<BuildingConfig, UpdatedBuildingConfigDto>().ReverseMap();
        CreateMap<BuildingConfig, BuildingConfigDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();
        CreateMap<BuildingConfig, BuildingConfigListDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id)) 
            .ReverseMap();
        CreateMap<IPaginate<BuildingConfig>, BuildingConfigListModel>().ReverseMap();
        CreateMap<BuildingConfig, BuildingTypeDto>()
            .ForMember(dest => dest.BuildingType, opt => opt.MapFrom(src => src.BuildingType))
            .ReverseMap();

    }
}
