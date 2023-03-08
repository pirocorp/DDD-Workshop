namespace CarRentalSystem.Fakes.Domain.Models.CarAds;

using System.Linq;

using CarRentalSystem.Domain.Models.CarAds;

using FakeItEasy;

public class CategoryFakes
{
    public class CategoryDummyFactory : DummyFactory<Category>
    {
        protected override Category Create() => new CategoryData().GetData().Cast<Category>().First();
    }
}
