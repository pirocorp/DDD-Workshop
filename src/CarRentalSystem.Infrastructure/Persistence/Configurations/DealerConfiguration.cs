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

        // Configure PhoneNumber(ValueObject) to be part of Dealer table
        // ValueObjects are part of given Entity 
        dealer
            .OwnsOne(
                d => d.PhoneNumber,
                p =>
                {
                    p.WithOwner();

                    // Configure that property Number of PhoneNumber ValueObject will be stored in the database
                    p.Property(pn => pn.Number);
                });

        // Because the CarAds collection is read-only, we need to explicitly tell
        // Entity Framework Core to use the private field by providing its name ("carAds").
        dealer
            .HasMany(d => d.CarAds)
            .WithOne()
            .Metadata
            .PrincipalToDependent // Configures Dealer to be principal of the relation e.g. CarAd to have DealerId as Foreign Key
            // If the Foreign Key filed was in Dealer should use DependentToPrincipal
            ?.SetField("carAds"); // Sets carAds filed on Dealer Entity
    }
}
