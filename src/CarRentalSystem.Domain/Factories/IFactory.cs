namespace CarRentalSystem.Domain.Factories;

using CarRentalSystem.Domain.Common;

public interface IFactory<out TEntity> 
    where TEntity : IAggregateRoot
{
    TEntity Build();
}
