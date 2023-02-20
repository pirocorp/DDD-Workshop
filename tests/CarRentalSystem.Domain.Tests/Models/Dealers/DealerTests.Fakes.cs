namespace CarRentalSystem.Domain.Tests.Models.Dealers;

using System;
using CarRentalSystem.Domain.Models.CarAds;
using CarRentalSystem.Domain.Models.Dealers;
using CarRentalSystem.Domain.Tests.Models.CarAds;
using FakeItEasy;

public class DealerFakes
{
    public class DealerDummyFactory : IDummyFactory
    {
        public bool CanCreate(Type type) => true;

        public object? Create(Type type)
        {
            if (type.Name == nameof(CarAd))
            {
                return new CarAdFakes.CarAdDummyFactory().Create(type);
            }
            
            return new Dealer("Dealer", "+359123456789");
        }

        public Priority Priority => Priority.Default;
    }
}
