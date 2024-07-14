using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Hashing;
using Domain.Concrete;
using MediatR;
using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Services.Repositories;

namespace Application.Features.Users.Command.CreateUser;

public class CreateUserCommand : IRequest<CreatedUserDto>, ISecuredRequest
{
    public string Username { get; set; }
    public bool Status { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserDto>
    {
        private readonly IUserRepository _userDal;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public CreateUserCommandHandler(IUserRepository userDal, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userDal = userDal;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<CreatedUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _userBusinessRules.UserEmailMustNotExist(request.Email);

            User mappedUser = _mapper.Map<User>(request);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
            mappedUser.PasswordHash = passwordHash;
            mappedUser.PasswordSalt = passwordSalt;

            mappedUser.RegistrationDate = Convert.ToDateTime(DateTime.Now.ToString("F"));
            User createdUser = await _userDal.AddAsync(mappedUser);
            CreatedUserDto createdUserDto = _mapper.Map<CreatedUserDto>(createdUser);

            return createdUserDto;

        }
    }
}
