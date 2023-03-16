namespace CarRentalSystem.Application.Features.CarAds;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Application.Features.CarAds.Queries.Search;
using CarRentalSystem.Domain.Models.CarAds;
using CarRentalSystem.Domain.Specifications;

public interface ICarAdRepository : IRepository<CarAd>
{
    Task<IEnumerable<CarAdListingModel>> GetCarAdListings(
        Specification<CarAd> specification,
        CancellationToken cancellationToken = default);

    Task<int> Total(CancellationToken cancellationToken = default);

    Task<Category?> GetCategory(int categoryId, CancellationToken cancellationToken);

    Task<Manufacturer?> GetManufacturer(string manufacturerName, CancellationToken cancellationToken);

    Task Save(CarAd carAd, CancellationToken cancellationToken);
}
