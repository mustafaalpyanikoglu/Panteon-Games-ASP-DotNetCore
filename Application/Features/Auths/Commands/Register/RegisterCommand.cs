using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Application.Services.AuthServices;
using Application.Services.Repositories;
using Application.Services.UserService;
using Core.Security.Hashing;
using Core.Security.Jwt;
using Domain.Concrete;
using MediatR;

namespace Application.Features.Auths.Commands.Register;

public class RegisterCommand : IRequest<RegisteredDto>
{
    public UserForRegisterDto UserForRegisterDto { get; set; }
    public string IPAddress { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly AuthBusinessRules _authBusinessRules;

        public RegisterCommandHandler(
            IUserRepository userDal, 
            IAuthService authService, 
            AuthBusinessRules authBusinessRules,
            IUserOperationClaimService userOperationClaimService
            )
        {
            _userRepository = userDal;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _userOperationClaimService = userOperationClaimService;
        }

        public async Task<RegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailShouldBeNotExists(request.UserForRegisterDto.Email);
            await _authBusinessRules.UsernameShouldBeNotExists(request.UserForRegisterDto.Username);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password, out passwordHash, out passwordSalt);
            User newUser = new()
            {
                Email = request.UserForRegisterDto.Email,
                Username = request.UserForRegisterDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                RegistrationDate = Convert.ToDateTime(DateTime.Now.ToString("F"))
            };
            User createdUser = await _userRepository.AddAsync(newUser);

            await _userOperationClaimService.AssignGamerRole(createdUser.Id);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);

            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(createdUser, request.IPAddress);
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredDto registeredDto = new()
                { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
            return registeredDto;
        }
    }
}