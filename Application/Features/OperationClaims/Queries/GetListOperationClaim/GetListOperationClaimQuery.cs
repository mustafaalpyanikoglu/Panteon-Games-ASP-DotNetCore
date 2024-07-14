using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Domain.Concrete;
using MediatR;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.OperationClaims.Models;
using Application.Services.Repositories;

namespace Application.Features.OperationClaims.Queries.GetListOperationClaim;

public class GetListOperationClaimQuery : IRequest<OperationClaimListModel>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class GetListOperationClaimQueryHanlder : IRequestHandler<GetListOperationClaimQuery, OperationClaimListModel>
    {
        private readonly IOperationClaimRepository _operationClaimDal;
        private readonly IMapper _mapper;

        public GetListOperationClaimQueryHanlder(IOperationClaimRepository operationClaimDal, IMapper mapper)
        {
            _operationClaimDal = operationClaimDal;
            _mapper = mapper;
        }

        public async Task<OperationClaimListModel> Handle(GetListOperationClaimQuery request, CancellationToken cancellationToken)
        {
            IPaginate<OperationClaim> operationClaims = await _operationClaimDal.GetListAsync(index: request.PageRequest.Page,
                                                                                              size: request.PageRequest.PageSize);
            OperationClaimListModel mappedOperationClaimListModel = _mapper.Map<OperationClaimListModel>(operationClaims);
            return mappedOperationClaimListModel;

        }
    }
}
