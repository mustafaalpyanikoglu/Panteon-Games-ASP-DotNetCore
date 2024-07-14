using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using static Application.Features.UserOperationClaims.Constants.OperationClaims;
using Application.Features.UserOperationClaims.Rules;
using Application.Features.OperationClaims.Rules;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.UserOperationClaims.Dtos;
using Application.Features.Users.Rules;

namespace Application.Features.UserOperationClaims.Commands.UpdateUserOperationClaim;

public class UpdateUserOperationClaimCommand : IRequest<UpdateUserOperationClaimDto>, ISecuredRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class UpdateUserOperationClaimCommandHandler : IRequestHandler<UpdateUserOperationClaimCommand, UpdateUserOperationClaimDto>
    {
        private readonly IUserOperationClaimRepository _userOperationClaimDal;
        private readonly IMapper _mapper;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

        public UpdateUserOperationClaimCommandHandler(IUserOperationClaimRepository userOperationClaimDal, IMapper mapper, UserOperationClaimBusinessRules userOperationClaimBusinessRules, UserBusinessRules userBusinessRules, OperationClaimBusinessRules operationClaimBusinessRules)
        {
            _userOperationClaimDal = userOperationClaimDal;
            _mapper = mapper;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
            _userBusinessRules = userBusinessRules;
            _operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<UpdateUserOperationClaimDto> Handle(UpdateUserOperationClaimCommand request, CancellationToken cancellationToken)
        {
            await _userOperationClaimBusinessRules.UserOperationClaimIdMustBeAvailable(request.Id);
            await _userBusinessRules.UserIdMustBeAvailable(request.UserId);
            await _operationClaimBusinessRules.OperationClaimIdShouldExistWhenSelected(request.OperationClaimId);

            UserOperationClaim mappedUserOperationClaim = _mapper.Map<UserOperationClaim>(request);
            UserOperationClaim updatedUserOperationClaim = await _userOperationClaimDal.UpdateAsync(mappedUserOperationClaim);
            UpdateUserOperationClaimDto updateUserOperationClaimDto = _mapper.Map<UpdateUserOperationClaimDto>(updatedUserOperationClaim);

            return updateUserOperationClaimDto;
        }
    }
}
