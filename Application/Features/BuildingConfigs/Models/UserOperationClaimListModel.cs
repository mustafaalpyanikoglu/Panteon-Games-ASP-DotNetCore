using Application.Features.BuildingConfigs.Dtos;
using Core.Persistence.Paging;

namespace Application.Features.UserOperationClaims.Models;

public class BuildingConfigListModel : BasePageableModel
{
    public IList<BuildingConfigListDto> Items { get; set; }
}
