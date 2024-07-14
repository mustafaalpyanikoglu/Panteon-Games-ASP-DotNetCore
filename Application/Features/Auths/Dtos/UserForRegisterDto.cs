using Core.Application.Dtos;

namespace Application.Features.Auths.Dtos;

public class UserForRegisterDto : IDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
