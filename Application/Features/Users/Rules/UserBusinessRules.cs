using Core.Security.Hashing;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions;
using static Application.Features.Users.Constants.UserMessages;
using Application.Services.Repositories;
using Domain.Concrete;

namespace Application.Features.Users.Rules;

public class UserBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userDal;

    public UserBusinessRules(IUserRepository userDal)
    {
        _userDal = userDal;
    }

    public async Task UserIdMustBeAvailable(int id)
    {
        User? result = await _userDal.GetAsync(t => t.Id == id);
        if (result == null) throw new BusinessException(UserNotFound);
    }
    public async Task ShouldNotHaveUserCart(int userId)
    {
        User? result = await _userDal.GetAsync(t => t.Id == userId);
        if (result != null) throw new BusinessException(ThisUserHasAUserCart);
    }
    public async Task ShouldNotHavePuser(int userId)
    {
        User? result = await _userDal.GetAsync(t => t.Id == userId);
        if (result != null) throw new BusinessException(ThisUserHasAPurse);
    }

    public async Task UserEmailMustBeAvailable(string email)
    {
        User? result = await _userDal.GetAsync(t => t.Email == email);
        if (result == null) throw new BusinessException(UserNotFound);
    }

    public async Task UserEmailMustNotExist(string email)
    {
        User? result = await _userDal.GetAsync(t => t.Email == email);
        if (result != null) throw new BusinessException(UserEmailAvaliable);
    }

    public async Task UserMustBeAvailable()
    {
        List<User>? results = _userDal.GetAll();
        if (results.Count <= 0) throw new BusinessException(UserNotFound);
    }

    public Task UserShouldBeExist(User? user)
    {
        if (user is null) throw new BusinessException(UserDontExists);
        return Task.CompletedTask;
    }

    public Task UserPasswordShouldBeMatch(User user, string password)
    {
        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(PasswordDontMatch);
        return Task.CompletedTask;
    }
}