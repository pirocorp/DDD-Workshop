namespace CarRentalSystem.Domain.Fakes.Models.CarAds;

using CarRentalSystem.Domain.Models.CarAds;

using FakeItEasy;

public class ManufacturerFakes
{
    public class ManufacturerDummyFactory : DummyFactory<Manufacturer>
    {
        protected override Manufacturer Create() => new ("Valid manufacturer");
    }
}
