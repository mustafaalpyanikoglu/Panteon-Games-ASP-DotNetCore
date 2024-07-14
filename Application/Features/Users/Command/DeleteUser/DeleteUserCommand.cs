using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;

namespace Application.Features.Users.Command.DeleteUser;

public class DeleteUserCommand : IRequest<DeletedUserDto>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeletedUserDto>
    {
        private readonly IUserRepository _userDal;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public DeleteUserCommandHandler(IUserRepository userDal, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userDal = userDal;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<DeletedUserDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserIdMustBeAvailable(request.Id);

            User mappedUser = _mapper.Map<User>(request);
            mappedUser.Status = false;
            User deletedUser = await _userDal.UpdateAsync(mappedUser);
            DeletedUserDto deletedUserDto = _mapper.Map<DeletedUserDto>(deletedUser);

            return deletedUserDto;
        }
    }
}
