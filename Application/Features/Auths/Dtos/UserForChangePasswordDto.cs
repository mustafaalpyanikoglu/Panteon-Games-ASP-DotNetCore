using Core.Application.Dtos;

namespace Application.Features.Auths.Dtos;

public class UserForChangePasswordDto : IDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
