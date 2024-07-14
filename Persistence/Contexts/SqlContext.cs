using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.EntityConfiguration;
using Persistence.EntityConfigurations;

namespace Persistence.Contexts;

public class SqlContext(DbContextOptions<SqlContext> options) : DbContext(options)
{
    protected IConfiguration Configuration { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new OperationClaimConfiguration());
        modelBuilder.ApplyConfiguration(new UserOperationClaimConfiguration());

        base.OnModelCreating(modelBuilder);
    }

}