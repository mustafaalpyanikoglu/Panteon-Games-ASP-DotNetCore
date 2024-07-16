using Core.Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Images.Dtos;

public class CreatedImageDto : IDto
{
    public int UserID { get; set; }
    public IFormFile Image { get; set; }
}
