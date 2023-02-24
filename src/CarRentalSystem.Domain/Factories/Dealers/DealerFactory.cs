namespace CarRentalSystem.Domain.Factories.Dealers;

using CarRentalSystem.Domain.Models.Dealers;

internal class DealerFactory : IDealerFactory
{
    private string dealerName = string.Empty;
    private string dealerPhoneNumber = string.Empty;

    public IDealerFactory WithName(string name)
    {
        this.dealerName = name;

        return this;
    }

    public IDealerFactory WithPhoneNumber(string phoneNumber)
    {
        this.dealerPhoneNumber = phoneNumber;

        return this;
    }

    public Dealer Build() => new (this.dealerName, this.dealerPhoneNumber);

    public Dealer Build(string name, string phoneNumber)
        => this
            .WithName(name)
            .WithPhoneNumber(phoneNumber)
            .Build();
}
