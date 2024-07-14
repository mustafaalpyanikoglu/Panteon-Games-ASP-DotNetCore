using Core.Application.Dtos;
using Core.Security.Jwt;
using Domain.Concrete;

namespace Application.Features.Auths.Dtos;

public class LoggedDto : IDto
{
    public AccessToken? AccessToken { get; set; }
    public RefreshToken? RefreshToken { get; set; }

    public LoggedResponseDto CreateResponseDto()
    {
        return new() { AccessToken = AccessToken};
    }

    public class LoggedResponseDto
    {
        public AccessToken? AccessToken { get; set; }
    }
}