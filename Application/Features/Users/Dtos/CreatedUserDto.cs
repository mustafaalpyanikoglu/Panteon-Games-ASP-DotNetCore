using Core.Application.Dtos;

namespace Application.Features.Users.Dtos;

public class CreatedUserDto : IDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
}
