namespace CarRentalSystem.Domain.Fakes.Models.Dealers;

using CarRentalSystem.Domain.Models.Dealers;

using FakeItEasy;

public class DealerFakes
{
    public class DealerDummyFactory : DummyFactory<Dealer>
    {
        protected override Dealer Create() => new ("Dealer", "+359123456789");
    }
}
