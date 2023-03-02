namespace CarRentalSystem.Fakes.Domain.Models.CarAds;

using CarRentalSystem.Domain.Models.CarAds;

using FakeItEasy;

public class CategoryFakes
{
    public class CategoryDummyFactory : DummyFactory<Category>
    {
        protected override Category Create() => new("Valid category", "Valid description text");
    }
}
