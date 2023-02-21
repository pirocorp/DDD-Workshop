namespace CarRentalSystem.Infrastructure.Persistence.Configurations;

using CarRentalSystem.Domain.Models.CarAds;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Domain.Models.ModelConstants.Common;
using static Domain.Models.ModelConstants.Category;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> category)
    {
        category
            .HasKey(c => c.Id);

        category
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(MAX_NAME_LENGTH);

        category
            .Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(MAX_DESCRIPTION_LENGTH);
    }
}
