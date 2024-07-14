﻿using Core.Application.Dtos;

namespace Application.Features.UserOperationClaims.Dtos;

public class UpdateUserOperationClaimDto : IDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }
}
