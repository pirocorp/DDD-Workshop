namespace CarRentalSystem.Infrastructure.Persistence.Configurations;

using CarRentalSystem.Domain.Models.CarAds;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static CarRentalSystem.Domain.Models.ModelConstants.Common;
using static CarRentalSystem.Domain.Models.ModelConstants.CarAd;

internal class CarAdConfiguration : IEntityTypeConfiguration<CarAd>
{
    public void Configure(EntityTypeBuilder<CarAd> carAd)
    {
        carAd
            .HasKey(c => c.Id);

        carAd
            .Property(c => c.Model)
            .IsRequired()
            .HasMaxLength(MAX_MODEL_LENGTH);

        carAd
            .Property(c => c.ImageUrl)
            .IsRequired()
            .HasMaxLength(MAX_URL_LENGTH);
        
        // The 2 arguments to HasPrecision(precision, scale) are poorly documented.
        // Precision is the total number of digits it will store, regardless of where the decimal point falls.
        // Scale is the number of decimal places it will store.
        // If you want to set the precision for all decimals use precision 38, scale 18
        carAd
            .Property(c => c.PricePerDay)
            .IsRequired()
            .HasPrecision(38, 18);

        carAd
            .Property(c => c.IsAvailable)
            .IsRequired();

        carAd
            .HasOne(c => c.Manufacturer)
            .WithMany()
            .HasForeignKey("ManufacturerId") // Define the foreign key that the EF and the Database will use but it's not present in Domain Model
            .OnDelete(DeleteBehavior.Restrict);

        carAd
            .HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey("CategoryId") // Define the foreign key that the EF and the Database will use but it's not present in Domain Model
            .OnDelete(DeleteBehavior.Restrict);

        // Specifies that Options(ValueObject) object properties are part of the CarAd Entity
        // ValueObjects do not have separate table they are part from the entity or another ValueObject 
        // In this case TransmissionType is ValueObject which is part of another ValueObject which is part of CarAd Table
        carAd
            .OwnsOne(c => c.Options, o =>
            {
                o.WithOwner();

                o.Property(op => op.NumberOfSeats);
                o.Property(op => op.HasClimateControl);

                // Nested ValueObject TransmissionType
                o.OwnsOne(op => op.TransmissionType, t =>
                {
                    t.WithOwner();

                    // This specifies that only the Value property of TransmissionType class will be stored in the database
                    t.Property(tr => tr.Value);
                });
            });
    }
}
