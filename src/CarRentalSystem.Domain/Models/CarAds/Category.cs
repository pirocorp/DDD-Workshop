namespace CarRentalSystem.Domain.Models.CarAds;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Exceptions;

using static CarRentalSystem.Domain.Models.ModelConstants.Common;
using static CarRentalSystem.Domain.Models.ModelConstants.Category;

public class Category : Entity<int>
{
    internal Category(string name, string description)
    {
        this.Validate(name, description);

        this.Name = name;
        this.Description = description;
    }

    public string Name { get; }

    public string Description { get; }

    private void Validate(string name, string description)
    {
        Guard.ForStringLength<InvalidCarAdException>(
            name,
            MIN_NAME_LENGTH,
            MAX_NAME_LENGTH,
            nameof(Category));

        Guard.ForStringLength<InvalidCarAdException>(
            description,
            MIN_DESCRIPTION_LENGTH,
            MAX_DESCRIPTION_LENGTH,
            nameof(this.Description));
    }
}
