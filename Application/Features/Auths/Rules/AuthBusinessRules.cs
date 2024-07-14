using Application.Features.Auths.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Security.Hashing;
using Domain.Concrete;

namespace Application.Features.Auths.Rules;

public class AuthBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;

    public AuthBusinessRules(IUserRepository userDal)
    {
        _userRepository = userDal;
    }

    public Task UserShouldBeExists(User? user)
    {
        if (user == null) throw new BusinessException(AuthMessages.UserDontExists);
        return Task.CompletedTask;
    }
    public Task PasswordsEnteredMustBeTheSame(string newPassword, string repeatPassword)
    {
        if (newPassword != repeatPassword) throw new BusinessException(AuthMessages.PasswordDoNotMatch);
        return Task.CompletedTask;
    }


    public async Task UserEmailShouldBeNotExists(string email)
    {
        User? user = await _userRepository.GetAsync(u => u.Email == email);
        if (user != null) throw new BusinessException(AuthMessages.UserEmailAlreadyExists);
    }

    public async Task UsernameShouldBeNotExists(string username)
    {
        User? user = await _userRepository.GetAsync(u => u.Username == username);
        if (user != null) throw new BusinessException(AuthMessages.UsernameAlreadyExists);
    }

    public async Task UserEmailMustBeAvailable(string email)
    {
        User? user = await _userRepository.GetAsync(u => u.Email == email);
        if (user == null) throw new BusinessException(AuthMessages.UserDontExists);
    }

    public async Task UserPasswordShouldBeMatch(int id, string password)
    {
        User? user = await _userRepository.GetAsync(u => u.Id == id);
        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDontMatch);
    }
    public Task RefreshTokenShouldBeActive(RefreshToken refreshToken)
    {
        if (refreshToken.Revoked != null && DateTime.UtcNow >= refreshToken.Expires)
            throw new BusinessException(AuthMessages.InvalidRefreshToken);
        return Task.CompletedTask;
    }
    public Task RefreshTokenShouldBeExists(RefreshToken? refreshToken)
    {
        if (refreshToken == null) throw new BusinessException(AuthMessages.RefreshDontExists);
        return Task.CompletedTask;
    }
}