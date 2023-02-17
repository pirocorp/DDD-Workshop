namespace CarRentalSystem.Domain.Models.CarAds;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Exceptions;

using static CarRentalSystem.Domain.Models.ModelConstants.Common;
using static CarRentalSystem.Domain.Models.ModelConstants.Category;

public class Category : Entity<int>
{
    internal Category(string description, string name)
    {
        this.Validate(name, description);

        this.Description = description;
        this.Name = name;
    }

    public string Description { get; }

    public string Name { get; }

    private void Validate(string name, string description)
    {
        Guard.ForStringLength<InvalidCarAdException>(
            name,
            MIN_NAME_LENGTH,
            MAX_NAME_LENGTH,
            nameof(this.Name));

        Guard.ForStringLength<InvalidCarAdException>(
            description,
            MIN_DESCRIPTION_LENGTH,
            MAX_DESCRIPTION_LENGTH,
            nameof(this.Description));
    }
}
