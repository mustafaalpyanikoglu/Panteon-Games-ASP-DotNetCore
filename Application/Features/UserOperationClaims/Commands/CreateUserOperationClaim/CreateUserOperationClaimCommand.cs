using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using Application.Features.OperationClaims.Rules;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.UserOperationClaims.Dtos;
using Application.Features.Users.Rules;

namespace Application.Features.UserOperationClaims.Commands.CreateUserOperationClaim;

public class CreateUserOperationClaimCommand : IRequest<CreateUserOperationClaimDto>, ISecuredRequest
{
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }

    public string[] Roles => new[] {ADMIN, GAMER, VIP };
    public class CreateUserOperationClaimCommandHanlder : IRequestHandler<CreateUserOperationClaimCommand, CreateUserOperationClaimDto>
    {
        private readonly IUserOperationClaimRepository _userOperationClaimDal;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly OperationClaimBusinessRules _operationClaimBusinessRules;

        public CreateUserOperationClaimCommandHanlder(IUserOperationClaimRepository userOperationClaimDal, IMapper mapper, UserBusinessRules userBusinessRules, OperationClaimBusinessRules operationClaimBusinessRules)
        {
            _userOperationClaimDal = userOperationClaimDal;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
            _operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<CreateUserOperationClaimDto> Handle(CreateUserOperationClaimCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserIdMustBeAvailable(request.UserId);
            await _operationClaimBusinessRules.OperationClaimIdShouldExistWhenSelected(request.OperationClaimId);

            UserOperationClaim mappedUserOperationClaim = _mapper.Map<UserOperationClaim>(request);
            UserOperationClaim createdUserOperationClaim = await _userOperationClaimDal.AddAsync(mappedUserOperationClaim);
            CreateUserOperationClaimDto createUserOperationClaimDto = _mapper.Map<CreateUserOperationClaimDto>(createdUserOperationClaim);

            return createUserOperationClaimDto;
        }
    }
}
