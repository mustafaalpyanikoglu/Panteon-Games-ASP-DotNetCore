using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;

namespace Application.Features.Users.Command.UpdateUser;

public class UpdateUserCommand : IRequest<UpdatedUserDto>, ISecuredRequest
{
    public int Id { get; set; }
    public string Username { get; set; }
    public bool Status { get; set; }
    public string Email { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdatedUserDto>
    {
        private readonly IUserRepository _userDal;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public UpdateUserCommandHandler(IUserRepository userDal, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userDal = userDal;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<UpdatedUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserIdMustBeAvailable(request.Id);

            User mappedUser = _mapper.Map<User>(request);

            User? user = await _userDal.GetAsync(u => u.Id == request.Id);
            mappedUser.PasswordHash = user.PasswordHash;
            mappedUser.PasswordSalt = user.PasswordSalt;

            User updatedUser = await _userDal.UpdateAsync(mappedUser);
            UpdatedUserDto updatedUserDto = _mapper.Map<UpdatedUserDto>(updatedUser);

            return updatedUserDto;
        }
    }
}
