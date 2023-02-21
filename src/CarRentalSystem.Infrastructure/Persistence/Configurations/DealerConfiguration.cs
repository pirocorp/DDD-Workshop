namespace CarRentalSystem.Infrastructure.Persistence.Configurations;

using CarRentalSystem.Domain.Models.Dealers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static CarRentalSystem.Domain.Models.ModelConstants.Common;

internal class DealerConfiguration : IEntityTypeConfiguration<Dealer>
{
    public void Configure(EntityTypeBuilder<Dealer> dealer)
    {
        dealer
            .HasKey(d => d.Id);

        dealer
            .Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(MAX_NAME_LENGTH);

        dealer
            .OwnsOne(
                d => d.PhoneNumber,
                p =>
                {
                    p.WithOwner();

                    p.Property(pn => pn.Number);
                });

        // Because the CarAds collection is read-only, we need to explicitly tell
        // Entity Framework Core to use the private field by providing its name ("carAds").
        dealer
            .HasMany(d => d.CarAds)
            .WithOne()
            .Metadata
            .PrincipalToDependent
            ?.SetField("carAds");
    }
}
