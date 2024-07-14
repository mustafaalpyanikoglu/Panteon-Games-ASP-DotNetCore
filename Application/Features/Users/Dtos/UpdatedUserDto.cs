using Core.Application.Dtos;

namespace Application.Features.Users.Dtos;

public class UpdatedUserDto : IDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public bool Status { get; set; }
    public string Email { get; set; }
}
