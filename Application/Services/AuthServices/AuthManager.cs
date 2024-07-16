using Application.Features.Auths.Rules;
using Core.Security.Hashing;
using Microsoft.EntityFrameworkCore;
using Core.Security.Jwt;
using Microsoft.Extensions.Configuration;
using Application.Services.Repositories;
using Domain.Concrete;
using static Application.Features.Auths.Constants.AuthMessages;
using static Application.Features.Users.Constants.UserMessages;
using Application.Features.Auths.Dtos;

namespace Application.Services.AuthServices;

public class AuthManager : IAuthService
{
    private readonly ITokenHelper _tokenHelper;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly IUserRepository _userRepository;
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly TokenOptions _tokenOptions;

    public AuthManager(ITokenHelper tokenHelper, AuthBusinessRules authBusinessRules,
        IUserRepository userDal, IUserOperationClaimRepository userOperationClaimDal,
        IRefreshTokenRepository refreshTokenDal, IConfiguration configuration)
    {
        _tokenHelper = tokenHelper;
        _authBusinessRules = authBusinessRules;
        _userRepository = userDal;
        _userOperationClaimRepository = userOperationClaimDal;
        _refreshTokenRepository = refreshTokenDal;
        _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
    }
    public async Task<User> ChangePassword(UserForChangePasswordDto userForChangePasswordDto)
    {
        byte[] passwordHash, passwordSalt;

        User? userResult = await _userRepository.GetAsync(u => u.Email == userForChangePasswordDto.Email);
        await _authBusinessRules.UserShouldBeExists(userResult);

        HashingHelper.CreatePasswordHash(userForChangePasswordDto.Password, out passwordHash, out passwordSalt);
        userResult.PasswordHash = passwordHash;
        userResult.PasswordSalt = passwordSalt;
        await _userRepository.UpdateAsync(userResult);

        return await Task.FromResult(userResult);
    }

    public async Task<AccessToken> CreateAccessToken(User user)
    {
        IList<OperationClaim> operationClaims = await _userOperationClaimRepository
                .Query()
                .AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .Select(p => new OperationClaim
                {
                    Id = p.OperationClaimId,
                    Name = p.OperationClaim.Name
                })
                .ToListAsync();

        AccessToken accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }

    public async Task<User> Login(UserForLoginDto userForLoginDto)
    {
        User? user = await _userRepository.GetAsync(u => u.Email == userForLoginDto.Username);
        await _authBusinessRules.UserShouldBeExists(user);
        await _authBusinessRules.UserPasswordShouldBeMatch(user.Id, userForLoginDto.Password);

        if (user == null)
        {
            return null;
        }
        if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password ?? "", user.PasswordHash, user.PasswordSalt))
        {
            return await Task.FromResult(user);
        }
        return await Task.FromResult(user);
    }

    public async Task<User> Register(UserForRegisterDto userForRegisterDto, string password)
    {
        await _authBusinessRules.UserEmailShouldBeNotExists(userForRegisterDto.Email);

        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
        User user = new User
        {
            Email = userForRegisterDto.Email,
            Username = userForRegisterDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Status = true,
            RegistrationDate = Convert.ToDateTime(DateTime.Now.ToString("F"))
        };

        await _userRepository.AddAsync(user);


        return await Task.FromResult(user);
    }
    public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
    {
        RefreshToken? refreshToken = await _refreshTokenRepository.GetAsync(r => r.Token == token);
        return refreshToken;
    }

    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        return Task.FromResult(refreshToken);
    }
    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress)
    {
        RefreshToken newRefreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        await RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
        return addedRefreshToken;
    }
    public async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress,
                                                string reason)
    {
        RefreshToken childToken = await _refreshTokenRepository.GetAsync(r => r.Token == refreshToken.ReplacedByToken);

        if (childToken != null && childToken.Revoked != null && childToken.Expires <= DateTime.UtcNow)
            await RevokeRefreshToken(childToken, ipAddress, reason);
        else await RevokeDescendantRefreshTokens(childToken, ipAddress, reason);
    }
    public async Task DeleteOldRefreshTokens(int userId)
    {
        IList<RefreshToken> refreshTokens = (await _refreshTokenRepository.GetListAsync(r =>
                                                r.UserId == userId &&
                                                r.Revoked == null && r.Expires >= DateTime.UtcNow &&
                                                r.Created.AddDays(_tokenOptions.RefreshTokenTTL) <=
                                                DateTime.UtcNow)
                                            ).Items;
        foreach (RefreshToken refreshToken in refreshTokens) await _refreshTokenRepository.DeleteAsync(refreshToken);
    }



    public async Task RevokeRefreshToken(RefreshToken refreshToken, string ipAddress, string? reason = null,
                                    string? replacedByToken = null)
    {
        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = reason;
        refreshToken.ReplacedByToken = replacedByToken;
        await _refreshTokenRepository.UpdateAsync(refreshToken);
    }
}
