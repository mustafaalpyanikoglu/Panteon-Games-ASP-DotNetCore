using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using Application.Features.OperationClaims.Models;
using static Core.Security.Constants.GeneralOperationClaims;

namespace Application.Features.OperationClaims.Queries.GetListOperationClaimByDynamic;

public class GetListOperationClaimByDynamicQuery : IRequest<OperationClaimListModel>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public Dynamic Dynamic { get; set; }
    public string[] Roles => new[] {ADMIN, GAMER, VIP };

    public class GetListOperationClaimByDynamicQueryHandler : IRequestHandler<GetListOperationClaimByDynamicQuery, OperationClaimListModel>
    {
        private readonly IOperationClaimRepository _operationClaimDal;
        private readonly IMapper _mapper;

        public GetListOperationClaimByDynamicQueryHandler(IOperationClaimRepository operationClaimDal, IMapper mapper)
        {
            _operationClaimDal = operationClaimDal;
            _mapper = mapper;
        }

        public async Task<OperationClaimListModel> Handle(GetListOperationClaimByDynamicQuery request, CancellationToken cancellationToken)
        {
            IPaginate<OperationClaim> operationClaims = await _operationClaimDal.GetListByDynamicAsync(
                                  request.Dynamic,
                                  null,
                                  request.PageRequest.Page,
                                  request.PageRequest.PageSize);
            OperationClaimListModel mappedOperationClaimListModel = _mapper.Map<OperationClaimListModel>(operationClaims);
            return mappedOperationClaimListModel;
        }
    }
}
