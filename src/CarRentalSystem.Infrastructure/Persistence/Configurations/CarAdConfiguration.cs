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

        carAd
            .Property(c => c.PricePerDay)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        carAd
            .Property(c => c.IsAvailable)
            .IsRequired();

        carAd
            .HasOne(c => c.Manufacturer)
            .WithMany()
            .HasForeignKey("ManufacturerId")
            .OnDelete(DeleteBehavior.Restrict);

        carAd
            .HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey("CategoryId")
            .OnDelete(DeleteBehavior.Restrict);

        carAd
            .OwnsOne(c => c.Options, o =>
            {
                o.WithOwner();

                o.Property(op => op.NumberOfSeats);
                o.Property(op => op.HasClimateControl);

                o.OwnsOne(
                    op => op.TransmissionType,
                    t =>
                    {
                        t.WithOwner();

                        t.Property(tr => tr.Value);
                    });
            });
    }
}
