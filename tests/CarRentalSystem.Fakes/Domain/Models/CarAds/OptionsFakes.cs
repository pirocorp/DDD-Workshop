namespace CarRentalSystem.Fakes.Domain.Models.CarAds;

using CarRentalSystem.Domain.Models.CarAds;

using FakeItEasy;

public class OptionsFakes
{
    public class OptionsDummyFactory : DummyFactory<Options>
    {
        protected override Options Create()
            => new(true, 4, TransmissionType.Automatic);
    }
}
