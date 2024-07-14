using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using static Core.Security.Constants.GeneralOperationClaims;

namespace Application.Features.Users.Queries.GetByIdUser;

public class GetByIdUserQuery : IRequest<UserDto>, ISecuredRequest
{
    public int Id { get; set; }
    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class GetByIdUserQueryHanlder : IRequestHandler<GetByIdUserQuery, UserDto>
    {
        private readonly IUserRepository _userDal;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public GetByIdUserQueryHanlder(IUserRepository userDal, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userDal = userDal;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<UserDto> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserIdMustBeAvailable(request.Id);

            User? user = await _userDal.GetAsync(u => u.Id == request.Id);
            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}
