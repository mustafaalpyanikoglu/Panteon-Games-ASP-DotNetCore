using Core.Application.Dtos;

namespace Application.Features.Users.Dtos;

public class UserDto : IDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public bool Status { get; set; }
    public DateTime RegistrationDate { get; set; }
}
