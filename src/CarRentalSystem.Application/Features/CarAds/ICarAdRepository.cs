namespace CarRentalSystem.Application.Features.CarAds;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Application.Features.CarAds.Queries.Search;
using CarRentalSystem.Domain.Models.CarAds;

public interface ICarAdRepository : IRepository<CarAd>
{
    Task<IEnumerable<CarAdListingModel>> GetCarAdListings(
        string? manufacturer = default,
        CancellationToken cancellationToken = default);

    Task<int> Total(CancellationToken cancellationToken = default);
}
