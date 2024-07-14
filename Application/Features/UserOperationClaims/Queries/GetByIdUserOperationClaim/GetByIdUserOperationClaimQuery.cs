using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Features.UserOperationClaims.Rules;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.UserOperationClaims.Dtos;

namespace Application.Features.UserOperationClaims.Queries.GetByIdUserOperationClaim;

public class GetByIdUserOperationClaimQuery : IRequest<UserOperationClaimDto>, ISecuredRequest
{
    public int Id { get; set; }
    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class GetByIdUserOperationClaimQueryHanlder : IRequestHandler<GetByIdUserOperationClaimQuery, UserOperationClaimDto>
    {
        private readonly IUserOperationClaimRepository _userOperationClaimDal;
        private readonly IMapper _mapper;
        private readonly UserOperationClaimBusinessRules _userOperationClaimBusinessRules;

        public GetByIdUserOperationClaimQueryHanlder(IUserOperationClaimRepository userOperationClaimDal, IMapper mapper, UserOperationClaimBusinessRules userOperationClaimBusinessRules)
        {
            _userOperationClaimDal = userOperationClaimDal;
            _mapper = mapper;
            _userOperationClaimBusinessRules = userOperationClaimBusinessRules;
        }

        public async Task<UserOperationClaimDto> Handle(GetByIdUserOperationClaimQuery request, CancellationToken cancellationToken)
        {
            await _userOperationClaimBusinessRules.UserOperationClaimIdMustBeAvailable(request.Id);

            UserOperationClaim? userOperationClaim = await _userOperationClaimDal.GetAsync(
                u => u.Id == request.Id,
                include: c => c.Include(c => c.User).Include(c => c.OperationClaim)
                );
            UserOperationClaimDto userOperationClaimDto = _mapper.Map<UserOperationClaimDto>(userOperationClaim);

            return userOperationClaimDto;
        }
    }
}
