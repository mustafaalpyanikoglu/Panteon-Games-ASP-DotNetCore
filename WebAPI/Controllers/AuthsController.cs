using Application.Features.Auths.Commands.ChangePassword;
using Application.Features.Auths.Commands.Login;
using Application.Features.Auths.Commands.RefleshToken;
using Application.Features.Auths.Commands.Register;
using Application.Features.Auths.Commands.RevokeToken;
using Application.Features.Auths.Dtos;
using Application.Services.AuthServices;
using Domain.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : BaseController
{
    private readonly IAuthService _authService;
    private readonly WebAPIConfiguration _configuration;

    public AuthsController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration.GetSection("WebAPIConfiguration").Get<WebAPIConfiguration>();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
    {
        LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto, IPAddress = getIpAddress() };
        LoggedDto result = await Mediator.Send(loginCommand);

        if (result.RefreshToken is not null) setRefreshTokenToCookie(result.RefreshToken);

        return Ok(result.CreateResponseDto());
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
    {
        RegisterCommand registerCommand = new() { UserForRegisterDto = userForRegisterDto, IPAddress = getIpAddress() };
        RegisteredDto result = await Mediator.Send(registerCommand);
        setRefreshTokenToCookie(result.RefreshToken);
        return Created("", result.AccessToken);
    }
    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        RefreshTokenCommand refreshTokenCommand = new()
        { RefleshToken = getRefreshTokenFromCookies(), IPAddress = getIpAddress() };
        RefreshedTokensDto result = await Mediator.Send(refreshTokenCommand);
        setRefreshTokenToCookie(result.RefreshToken);
        return Created("", result.AccessToken);
    }


    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken(
    [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
    string? refreshToken)
    {
        RevokeTokenCommand revokeTokenCommand = new()
        {
            Token = refreshToken ?? getRefreshTokenFromCookies(),
            IPAddress = getIpAddress()
        };
        RevokedTokenDto result = await Mediator.Send(revokeTokenCommand);
        return Ok(result);
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> Add([FromBody] ChangePasswordCommand changePasswordCommand)
    {
        UserForChangePasswordDto result = await Mediator.Send(changePasswordCommand);
        return Ok(result);
    }
    private string? getRefreshTokenFromCookies()
    {
        return Request.Cookies["refreshToken"];
    }

    private void setRefreshTokenToCookie(RefreshToken refreshToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
}
