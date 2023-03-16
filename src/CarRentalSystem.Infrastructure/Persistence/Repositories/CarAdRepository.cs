namespace CarRentalSystem.Infrastructure.Persistence.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CarRentalSystem.Application.Features.CarAds;
using CarRentalSystem.Application.Features.CarAds.Queries.Search;
using CarRentalSystem.Domain.Models.CarAds;
using CarRentalSystem.Domain.Specifications;

using Microsoft.EntityFrameworkCore;

internal class CarAdRepository : DataRepository<CarAd>, ICarAdRepository
{
    private readonly IMapper mapper;

    public CarAdRepository(
        CarRentalDbContext dbContext,
        IMapper mapper) 
        : base(dbContext)
    {
        this.mapper = mapper;
    }

    public async Task<IEnumerable<CarAdListingModel>> GetCarAdListings(
        Specification<CarAd> specification,
        CancellationToken cancellationToken = default)
        => await this.AllAvailable()
            .Where(specification)
            .ProjectTo<CarAdListingModel>(this.mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task<int> Total(CancellationToken cancellationToken = default)
        => await this
            .AllAvailable()
            .CountAsync(cancellationToken);

    public async Task<Category?> GetCategory(int categoryId, CancellationToken cancellationToken = default)
        => await this.Data.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

    public async Task<Manufacturer?> GetManufacturer(
        string manufacturer,
        CancellationToken cancellationToken = default)
        => await this.Data.Manufacturers
            .FirstOrDefaultAsync(m => m.Name == manufacturer, cancellationToken);

    private IQueryable<CarAd> AllAvailable()
        => this
            .All()
            .Where(car => car.IsAvailable);
}
