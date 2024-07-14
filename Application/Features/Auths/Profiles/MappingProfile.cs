using Application.Features.Auths.Commands.ChangePassword;
using Application.Features.Auths.Dtos;
using AutoMapper;
using Domain.Concrete;

namespace Application.Features.Auths.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, ChangePasswordCommand>().ReverseMap();
        CreateMap<User, UserForChangePasswordDto>().ReverseMap();
        CreateMap<RefreshToken, RevokedTokenDto>().ReverseMap();
    }
}
