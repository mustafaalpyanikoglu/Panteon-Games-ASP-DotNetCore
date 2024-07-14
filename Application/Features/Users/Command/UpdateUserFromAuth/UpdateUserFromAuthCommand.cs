using Application.Services.AuthService;
using AutoMapper;
using Core.Security.Hashing;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using static Application.Features.Users.Constants.OperationClaims;
using Application.Features.Users.Dtos;
using Application.Features.Users.Rules;

namespace Application.Features.Users.Command.UpdateUserFromAuth;

public class UpdateUserFromAuthCommand : IRequest<UpdatedUserFromAuthDto>
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string? NewPassword { get; set; }


    public class UpdateUserFromAuthCommandHandler : IRequestHandler<UpdateUserFromAuthCommand, UpdatedUserFromAuthDto>
    {
        private readonly IUserRepository _userDal;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;
        private readonly IAuthService _authService;

        public UpdateUserFromAuthCommandHandler(IUserRepository userDal, IMapper mapper, UserBusinessRules userBusinessRules, IAuthService authService)
        {
            _userDal = userDal;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
            _authService = authService;
        }

        public async Task<UpdatedUserFromAuthDto> Handle(UpdateUserFromAuthCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userDal.GetAsync(u => u.Id == request.Id);
            await _userBusinessRules.UserShouldBeExist(user);
            await _userBusinessRules.UserPasswordShouldBeMatch(user, request.Password);

            if (request.NewPassword is not null && !string.IsNullOrWhiteSpace(request.NewPassword))
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(request.NewPassword, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            User updatedUser = await _userDal.UpdateAsync(user);
            UpdatedUserFromAuthDto updatedUserFromAuthDto = _mapper.Map<UpdatedUserFromAuthDto>(updatedUser);
            updatedUserFromAuthDto.AccessToken = await _authService.CreateAccessToken(user);

            return updatedUserFromAuthDto;
        }
    }
}
