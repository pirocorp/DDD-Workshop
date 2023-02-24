namespace CarRentalSystem.Domain.Factories.Dealers;

using CarRentalSystem.Domain.Models.Dealers;

public interface IDealerFactory : IFactory<Dealer>
{
    Dealer Build(string name, string phoneNumber);

    IDealerFactory WithName(string name);

    IDealerFactory WithPhoneNumber(string phoneNumber);
}
