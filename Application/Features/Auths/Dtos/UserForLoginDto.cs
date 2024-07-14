using Core.Application.Dtos;

namespace Application.Features.Auths.Dtos;

public class UserForLoginDto : IDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}
