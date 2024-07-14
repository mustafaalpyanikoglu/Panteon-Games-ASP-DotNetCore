using Core.Application.Dtos;
using Core.Security.Jwt;
using Domain.Concrete;

namespace Application.Features.Auths.Dtos;

public class RefreshedTokensDto : IDto
{
    public AccessToken AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}