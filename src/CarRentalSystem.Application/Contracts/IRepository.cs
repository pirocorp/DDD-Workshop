namespace CarRentalSystem.Application.Contracts;

using System.Threading;
using System.Threading.Tasks;

using CarRentalSystem.Domain.Common;

/// <summary>
/// Markup interface which is used to mark repositories and they will be automatically registered in DI container
/// </summary>
/// <remarks>
/// Interface is limited for only Aggregate Root Entities
/// </remarks>
/// <typeparam name="TEntity">Aggregate Root</typeparam>
public interface IRepository<in TEntity> where TEntity : IAggregateRoot
{
    Task Save(TEntity entity, CancellationToken cancellationToken = default);
}
