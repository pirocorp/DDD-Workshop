namespace CarRentalSystem.Infrastructure.Persistence.Repositories;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CarRentalSystem.Application.Features.Dealers;
using CarRentalSystem.Domain.Exceptions;
using CarRentalSystem.Domain.Models.Dealers;

using Microsoft.EntityFrameworkCore;

internal class DealerRepository : DataRepository<Dealer>, IDealerRepository
{
    public DealerRepository(CarRentalDbContext dbContext) 
        : base(dbContext)
    { }

    public async Task<Dealer> FindByUser(string userId, CancellationToken cancellationToken)
    {
        var dealer = await this.Data.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Dealer)
            .FirstOrDefaultAsync(cancellationToken);

        if (dealer is null)
        {
            throw new InvalidDealerException("This user is not a dealer.");
        }

        return dealer;
    }
}
