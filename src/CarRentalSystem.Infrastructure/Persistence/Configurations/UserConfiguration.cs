namespace CarRentalSystem.Infrastructure.Persistence.Configurations;

using CarRentalSystem.Infrastructure.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> user)
    {
        user
            .HasOne(u => u.Dealer)
            .WithOne()
            .HasForeignKey<User>("DealerId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
