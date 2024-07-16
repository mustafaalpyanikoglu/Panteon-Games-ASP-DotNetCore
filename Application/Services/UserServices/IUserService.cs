using Domain.Concrete;

namespace Application.Services.UserServices;

public interface IUserService
{
    public Task<User?> GetByEmail(string email);
    public Task<User?> GetByUsername(string email);
    public Task<User> GetById(int id);
    public Task<User> Update(User user);
}
