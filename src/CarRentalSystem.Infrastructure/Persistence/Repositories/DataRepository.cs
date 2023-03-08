namespace CarRentalSystem.Infrastructure.Persistence.Repositories;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Domain.Common;

internal abstract class DataRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    protected DataRepository(CarRentalDbContext dbContext)
    {
        this.Data = dbContext;
    }

    protected CarRentalDbContext Data { get; }

    public async Task Save(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        this.Data.Add(entity);

        await this.Data.SaveChangesAsync(cancellationToken);
    }

    protected IQueryable<TEntity> All() => this.Data.Set<TEntity>();
}
