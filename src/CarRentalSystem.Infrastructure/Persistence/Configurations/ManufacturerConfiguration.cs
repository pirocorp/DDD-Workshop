namespace CarRentalSystem.Infrastructure.Persistence.Configurations;

using CarRentalSystem.Domain.Models.CarAds;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static CarRentalSystem.Domain.Models.ModelConstants.Common;

internal class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> manufacturer)
    {
        manufacturer
            .HasKey(m => m.Id);

        manufacturer
            .Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(MAX_NAME_LENGTH);
    }
}
