namespace CarRentalSystem.Domain.Models.CarAds;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Exceptions;

using static CarRentalSystem.Domain.Models.ModelConstants.Common;

public class Manufacturer : Entity<int>
{
    internal Manufacturer(string name)
    {
        this.Validate(name);

        this.Name = name;
    }

    public string Name { get; }

    public void Validate(string name)
        => Guard.ForStringLength<InvalidCarAdException>(
            name,
            MIN_NAME_LENGTH,
            MAX_NAME_LENGTH,
            nameof(Manufacturer));
}
