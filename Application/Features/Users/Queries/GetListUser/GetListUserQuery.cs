using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.Users.Models;

namespace Application.Features.Users.Queries.GetListUser;

public class GetListUserQuery : IRequest<UserListModel>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public string[] Roles => new[] {ADMIN, GAMER, VIP };

    public class GetListUserQueryHandler : IRequestHandler<GetListUserQuery, UserListModel>
    {
        private readonly IUserRepository _userDal;
        private readonly IMapper _mapper;

        public GetListUserQueryHandler(IUserRepository userDal, IMapper mapper)
        {
            _userDal = userDal;
            _mapper = mapper;
        }

        public async Task<UserListModel> Handle(GetListUserQuery request, CancellationToken cancellationToken)
        {
            IPaginate<User> users = await _userDal.GetListAsync(index: request.PageRequest.Page,
                                                                size: request.PageRequest.PageSize);
            UserListModel mappedUserListModel = _mapper.Map<UserListModel>(users);
            return mappedUserListModel;
        }
    }
}
