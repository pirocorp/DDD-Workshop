namespace CarRentalSystem.Infrastructure.Persistence.Repositories;

using System.Linq;

using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Domain.Common;

internal abstract class DataRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    private readonly CarRentalDbContext database;

    protected DataRepository(CarRentalDbContext database)
    {
        this.database = database;
    }

    protected IQueryable<TEntity> All() => this.database.Set<TEntity>();
}
