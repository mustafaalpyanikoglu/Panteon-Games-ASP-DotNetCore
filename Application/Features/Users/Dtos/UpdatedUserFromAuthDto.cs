using Core.Application.Dtos;
using Core.Security.Jwt;

namespace Application.Features.Users.Dtos;

public class UpdatedUserFromAuthDto : IDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public AccessToken AccessToken { get; set; }
}
