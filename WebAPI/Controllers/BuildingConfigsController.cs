using Application.Features.BuildingConfigs.Commands;
using Application.Features.BuildingConfigs.Commands.CreateBuildingConfig;
using Application.Features.BuildingConfigs.Commands.DeleteBuildingConfig;
using Application.Features.BuildingConfigs.Dtos;
using Application.Features.BuildingConfigs.Queries.GetBuildingTypeList;
using Application.Features.BuildingConfigs.Queries.GetByIdBuildingConfig;
using Application.Features.BuildingConfigs.Queries.GetListBuildingConfig;
using Application.Features.BuildingConfigs.Queries.GetListBuildingConfigWithPagination;
using Core.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BuildingConfigsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add(CreateBuildingConfigCommand createBuildingConfigCommand)
    {
        CreatedBuildingConfigDto result = await Mediator.Send(createBuildingConfigCommand);
        return Created("", result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteBuildingConfigCommand deleteBuildingConfigCommand)
    {
        DeletedBuildingConfigDto result = await Mediator.Send(deleteBuildingConfigCommand);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateBuildingConfigCommand updateBuildingConfigCommand)
    {
        UpdatedBuildingConfigDto result = await Mediator.Send(updateBuildingConfigCommand);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        GetListBuildingConfigQuery getListBuildingConfigQuery = new() { };
        var result = await Mediator.Send(getListBuildingConfigQuery);
        return Ok(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetBuildingTypeList()
    {
        GetBuildingTypeListQuery getListBuildingConfigQuery = new() { };
        var result = await Mediator.Send(getListBuildingConfigQuery);
        return Ok(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetListWithPagination([FromQuery] PageRequest pageRequest)
    {
        GetListBuildingConfigWithPaginationQuery getListBuildingConfigWithPagination = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListBuildingConfigWithPagination);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        GetByIdBuildingConfigQuery getByIdBuildingConfigQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdBuildingConfigQuery);
        return Ok(result);
    }
}
