using Application.Features.UserOperationClaims.Commands.CreateUserOperationClaim;
using Application.Features.UserOperationClaims.Commands.DeleteUserOperationClaim;
using Application.Features.UserOperationClaims.Commands.UpdateUserOperationClaim;
using Application.Features.UserOperationClaims.Dtos;
using Application.Features.UserOperationClaims.Models;
using AutoMapper;
using Core.Persistence.Paging;
using Domain.Concrete;

namespace Application.Features.UserOperationClaims.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserOperationClaim, CreateUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, DeleteUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, UpdateUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, CreateUserOperationClaimDto>().ReverseMap();
        CreateMap<UserOperationClaim, DeleteUserOperationClaimDto>().ReverseMap();
        CreateMap<UserOperationClaim, UpdateUserOperationClaimDto>().ReverseMap();
        CreateMap<UserOperationClaim, UserOperationClaimDto>().ForMember(t => t.OperationClaimName, opt => opt.MapFrom(u => u.OperationClaim.Name))
                                                                  .ForMember(t => t.OperationClaimNameDescription, opt => opt.MapFrom(u => u.OperationClaim.Description))
                                                                  .ReverseMap();
        CreateMap<UserOperationClaim, UserOperationClaimListDto>().ForMember(t => t.OperationClaimName, opt => opt.MapFrom(u => u.OperationClaim.Name))
                                                                  .ForMember(t => t.OperationClaimNameDescription, opt => opt.MapFrom(u => u.OperationClaim.Description))
                                                                  .ReverseMap();
        CreateMap<IPaginate<UserOperationClaim>, UserOperationClaimListModel>().ReverseMap();
    }
}
