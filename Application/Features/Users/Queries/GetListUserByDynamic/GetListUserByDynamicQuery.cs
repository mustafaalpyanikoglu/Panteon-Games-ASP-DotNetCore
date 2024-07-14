using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.Users.Models;

namespace Application.Features.Users.Queries.GetListUserByDynamic;

public class GetListUserByDynamicQuery : IRequest<UserListModel>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public Dynamic Dynamic { get; set; }
    public string[] Roles => new[] {ADMIN, GAMER, VIP };

    public class GetListUserByDynamicQueryHandler : IRequestHandler<GetListUserByDynamicQuery, UserListModel>
    {
        private readonly IUserRepository _userDal;
        private readonly IMapper _mapper;

        public GetListUserByDynamicQueryHandler(IUserRepository userDal, IMapper mapper)
        {
            _userDal = userDal;
            _mapper = mapper;
        }

        public async Task<UserListModel> Handle(GetListUserByDynamicQuery request, CancellationToken cancellationToken)
        {
            IPaginate<User> userOperationClaims = await _userDal.GetListByDynamicAsync(
                                  request.Dynamic,
                                  include: c => c.Include(c => c.UserOperationClaims),
                                  request.PageRequest.Page,
                                  request.PageRequest.PageSize);
            UserListModel mappedUserListModel = _mapper.Map<UserListModel>(userOperationClaims);
            return mappedUserListModel;
        }
    }
}
