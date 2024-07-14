using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfiguration;

public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
{
    public void Configure(EntityTypeBuilder<OperationClaim> builder)
    {
        #region OperationClaim Model Creation
        builder.ToTable("OperationClaims").HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("Id").UseIdentityColumn(1, 1);
        builder.Property(u => u.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
        builder.Property(u => u.Description).HasColumnName("Description").HasMaxLength(500).IsRequired();
        #endregion

        OperationClaim[] operationClaimSeeds =
        {
            new(1, "admin","Bütün işlemleri yapabilir."),
            new(2, "gamer","Standart Oyuncu"),
            new(3, "vip", "VIP Oyuncu")
        };
        builder.HasData(operationClaimSeeds);
    }
}
