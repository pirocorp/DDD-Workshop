namespace CarRentalSystem.Application.Contracts;

using CarRentalSystem.Domain.Common;

/// <summary>
/// Markup interface which is used to mark repositories and they will be automatically registered in DI container
/// </summary>
/// <remarks>
/// Interface is limited for only Aggregate Root Entities
/// </remarks>
/// <typeparam name="TEntity">Aggregate Root</typeparam>
public interface IRepository<out TEntity> where TEntity : IAggregateRoot
{ }
