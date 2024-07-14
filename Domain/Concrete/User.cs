using Core.Persistence.Repositories;

namespace Domain.Concrete;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool Status { get; set; }


    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }


    public User()
    {
        UserOperationClaims = new HashSet<UserOperationClaim>();
        RefreshTokens = new HashSet<RefreshToken>();
    }

    public User(int id, string username, string email, byte[] passwordHash, byte[] passwordSalt, DateTime registrationDate, bool status) : this()
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        RegistrationDate = registrationDate;
        Status = status;
    }
}
