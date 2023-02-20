namespace CarRentalSystem.Domain.Tests.Models.CarAds;

using System;
using CarRentalSystem.Domain.Models.CarAds;

using FakeItEasy;

public class CarAdFakes
{
    public class CarAdDummyFactory : IDummyFactory
    {
        public bool CanCreate(Type type) => true;

        public object? Create(Type type) 
            => new CarAd(
                new Manufacturer("Valid manufacturer"),
                "Valid model",
                new Category("Valid Category", "Valid description text"),
                "https://piro.com",
                10,
                new Options(true, 4, TransmissionType.Automatic),
                true);

        public Priority Priority => Priority.Default;
    }
}
