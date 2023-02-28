namespace CarRentalSystem.Application.Contracts;

using CarRentalSystem.Domain.Common;

public interface IRepository<out TEntity> where TEntity : IAggregateRoot
{ }
