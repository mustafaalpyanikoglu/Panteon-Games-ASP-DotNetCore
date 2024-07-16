using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Application.Services.AuthServices;
using Application.Services.UserServices;
using Domain.Concrete;
using MediatR;
using AccessToken = Core.Security.Jwt.AccessToken;

namespace Application.Features.Auths.Commands.Login;

public class LoginCommand : IRequest<LoggedDto>
{
    public UserForLoginDto UserForLoginDto { get; set; }
    public string IPAddress { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedDto>
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly AuthBusinessRules _authBusinessRules;

        public LoginCommandHandler(IUserService userService, IAuthService authService, AuthBusinessRules authBusinessRules)
        {
            _userService = userService;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<LoggedDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userService.GetByUsername(request.UserForLoginDto.Username);
            await _authBusinessRules.UserShouldBeExists(user);
            await _authBusinessRules.UserPasswordShouldBeMatch(user.Id, request.UserForLoginDto.Password);

            LoggedDto loggedDto = new();

            AccessToken createdAccessToken = await _authService.CreateAccessToken(user);

            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(user, request.IPAddress);
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);
            await _authService.DeleteOldRefreshTokens(user.Id);

            loggedDto.AccessToken = createdAccessToken;
            loggedDto.RefreshToken = addedRefreshToken;
            return loggedDto;
        }
    }
}
