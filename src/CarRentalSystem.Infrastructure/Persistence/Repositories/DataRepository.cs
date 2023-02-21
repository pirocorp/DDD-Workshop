namespace CarRentalSystem.Infrastructure.Persistence.Repositories;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Domain.Common;

internal class DataRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    private readonly CarRentalDbContext database;

    public DataRepository(CarRentalDbContext database)
    {
        this.database = database;
    }

    public IQueryable<TEntity> All() => this.database.Set<TEntity>();

    public Task<int> SaveChanges(CancellationToken cancellationToken = default)
        => this.database.SaveChangesAsync(cancellationToken);
}
