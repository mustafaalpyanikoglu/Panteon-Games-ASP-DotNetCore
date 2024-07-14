using Application.Features.Auths.Dtos;
using Core.Security.Jwt;
using Domain.Concrete;

namespace Application.Services.AuthService;

public interface IAuthService
{
    public Task<User> Register(UserForRegisterDto userForRegisterDto, string password);
    public Task<User> Login(UserForLoginDto userForLoginDto);
    public Task<RefreshToken?> GetRefreshTokenByToken(string token);
    public Task<User> ChangePassword(UserForChangePasswordDto userForChangePasswordDto);
    public Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress);
    public Task DeleteOldRefreshTokens(int userId);
    public Task<AccessToken> CreateAccessToken(User user);
    public Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason);
    public Task RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null,
                            string? replacedByToken = null);
    public Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress);
}
