using Application.Features.OperationClaims.Commands;
using Application.Features.OperationClaims.Commands.CreateOperationClaim;
using Application.Features.OperationClaims.Commands.DeleteOperationClaim;
using Application.Features.OperationClaims.Dtos;
using Application.Features.OperationClaims.Models;
using Application.Features.OperationClaims.Queries.GetByIdOperationClaim;
using Application.Features.OperationClaims.Queries.GetListOperationClaim;
using Application.Features.OperationClaims.Queries.GetListOperationClaimByDynamic;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperationClaimsController : BaseController
{
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CreateOperationClaimCommand createOperationClaimCommand)
    {
        CreatedOperationClaimDto result = await Mediator.Send(createOperationClaimCommand);
        return Created("", result);
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteOperationClaimCommand deleteOperationClaimCommand)
    {
        DeletedOperationClaimDto result = await Mediator.Send(deleteOperationClaimCommand);
        return Ok(result);
    }
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateOperationClaimCommand updateOperationClaimCommand)
    {
        UpdatedOperationClaimDto result = await Mediator.Send(updateOperationClaimCommand);
        return Ok(result);
    }
    [HttpGet("getlist")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListOperationClaimQuery getListOperationClaimQuery = new() { PageRequest = pageRequest };
        OperationClaimListModel result = await Mediator.Send(getListOperationClaimQuery);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        GetByIdOperationClaimQuery getByIdOperationClaimQuery = new() { Id = id };
        OperationClaimDto result = await Mediator.Send(getByIdOperationClaimQuery);
        return Ok(result);
    }
    [HttpPost("getlist/bydynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest,
                                                  [FromBody] Dynamic? dynamic = null)
    {
        GetListOperationClaimByDynamicQuery getListOperationClaimByDynamicQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
        OperationClaimListModel result = await Mediator.Send(getListOperationClaimByDynamicQuery);
        return Ok(result);
    }
}
