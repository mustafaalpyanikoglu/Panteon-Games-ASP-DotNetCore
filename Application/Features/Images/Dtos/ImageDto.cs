using Core.Application.Dtos;

namespace Application.Features.Images.Dtos;

public class ImageDto : IDto
{
    public string ImageUrl { get; set; }
}
