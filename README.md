# DDD Workshop

[![.NET](https://github.com/pirocorp/DDD-Workshop/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/pirocorp/DDD-Workshop/actions/workflows/dotnet.yml)
[![codecov](https://codecov.io/gh/pirocorp/DDD-Workshop/branch/main/graph/badge.svg?token=00E68NLAGS)](https://codecov.io/gh/pirocorp/DDD-Workshop)

## System Requirements

Design a system in which car dealers can publish their cars for rent.

Each car ad must contain manufacturer, model, category, image, and price per day. Categories have a description and must be one of the following - economy, compact, estate, minivan, SUV, and cargo van. Additionally, each vehicle should list the following options: with or without climate control, number of seats, and transmission type.

The system should allow users to filter the cars by category, manufacturer, and price range anonymously. Ads can be sorted by manufacturer or by price.

When a user chooses a car, he needs to call the dealer on the provided phone and make the arrangement. The dealer then needs to edit the car ad as `currently unavailable` manually. The system must not show the unavailable cars.


## Defining the Initial Domain Model

- **Dealer Aggregate**
  - **Dealer** - **Entity**, **Aggregate Root**
  - **Phone Number** - **Value Object**, part of **Dealer Aggregate** 
- **Car Ad Aggregate**
  - **Car Ad** - **Entity**, **Aggregate Root**
  - **Car Manufacturer** - **Entity**, part of **Car Ad Aggregate**
  - **Car Category** - **Entity**, part of **Car Ad Aggregate**
  - **Car Options** - **Value Object**, part of **Car Ad Aggregate** 

### Define Some Base Classes

- **Entity** – contains common logic for entities – identifier and equality (example for [Identity Based Equality](https://github.com/pirocorp/Object-Oriented-Design/tree/main/12.%20Other%20Patterns/02.%20Identity%20Based%20Equality))
- **ValueObject** – contains common logic for value objects - equality (more info for [Value Object Pattern](https://github.com/pirocorp/Object-Oriented-Design/tree/main/13.%20DDD/Value%20Object))
- **Enumeration** – contains common enumeration methods (more info at [Use enumeration classes instead of enum types](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types))
- **Guard** – contains common validation methods
- **BaseDomainException** – a base class for all domain exceptions
- **IAggregateRoot** - an empty marker interface, which all **Aggregate roots** will implement

Put **Aggregates** in separate subfolders in the `Models` folder. This approach makes sense since **Aggregate** parts are logically connected. All other objects will be put in the main `Models` folder. Create an empty `IAggregateRoot` marker interface, which all **Aggregate roots** will implement.

### Implementing the Aggregates

Make sure you follow the general DDD rules:
- Do not confuse domain objects with database schema and entities. Try not to think about any persistence layer while you design the classes.
- All domain objects should be immutable and read-only through their properties. Do not expose setters or whole collections.
- Constructors should create a valid object in terms of state.
- Only the aggregate root constructors should be public. The others should be internal.
- All mutating operations and behaviors should be done through methods.
- Do not create two-way relationships. Relationships should be based on the business domain and the logic behind them.
- Create an exception class for each domain aggregate. Do not use generic exceptions to indicate domain-related errors.
- Mark every object as an entity, a value objects, or an enumeration. Mark the aggregate roots as well.
- Do not seek perfection. The classes will evolve as we add additional layers to our solution. Additionally, we may need to modify our implementations when there are new or changed project requirements.

Here is a potential implementation of the project structure:

![image](https://user-images.githubusercontent.com/34960418/219692741-27cb539e-4820-4ff6-a17c-caa723ed4d76.png)


## Domain Model Unit Tests

The **domain model is perfect for unit tests**. The usual logic under test is the one with the object validation and the state mutability operations. 

To test the internal members, you need to add the `[assembly: InternalsVisibleTo("Logic.Tests")]` attribute in `AssemblyInfo.cs`. You can add one with the ***Add New Item*** dialog and set the attribute.

![image](https://user-images.githubusercontent.com/34960418/219699608-bf4b287c-1613-4baa-8706-bf57c03e2cb4.png)


## Presentation Layer

Add an `ASP.NET API` project to the solution and name it `CarRentalSystem.Startup`. This project contains just bootstrapping logic. Delete everything except `Program.cs`, `appsettings.json`, and `launchSettings.json` (in the `Properties` folder). 

Then, create another project – а new .NET class library. Name it `CarRentalSystem.Web`. This project will contain **HTTP** request-response logic. Reference it by **Startup**, add `Features` folder in it, and add `CarAdsController`.

![image](https://user-images.githubusercontent.com/34960418/220319552-ea3ffcd2-5158-4c6d-8186-78abe4648b6d.png)

You will need to reference the [ASP.NET Core framework](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/target-aspnetcore?view=aspnetcore-7.0&tabs=visual-studio#use-the-aspnet-core-shared-framework) and the Domain project to create the controller.

Create test action in `CarAdsController`:

```csharp
[ApiController]
[Route("[controller]")]
public class CarAdsController : ControllerBase
{
    private static readonly Dealer Dealer = new ("Dealer", "+359123456789");

    [HttpGet]
    public IEnumerable<CarAd> Get() => Dealer
        .CarAds
        .Where(c => c.IsAvailable);
}
```

The [Swagger](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio) (if configured) should show you the new action: 

![image](https://user-images.githubusercontent.com/34960418/220334732-83c8985b-0bff-45ce-98e1-1c1101a48311.png)


## Infrastructure Layer and Persistence

Using an Object–relational mapping (ORM) with the domain model may force us to make some modifications to the underlying classes, and that is entirely OK if we follow the main rules of domain-driven design:

- Keep the domain models immutable & read-only. If mutability is needed – it is better to create a separate class just for the data layer.
- Do not add ORM-specific logic to the domain objects – data annotations, for example. These attributes should not be used in DDD. Use the Entity Framework fluent configuration instead.

Following [Clean architecture](https://github.com/pirocorp/Object-Oriented-Design/blob/main/11.%20Architectural%20Patterns/CHO%20Architecture.md) - the persistence logic and all other third-party dependencies should be part of the **infrastructure** layer:

![image](https://user-images.githubusercontent.com/34960418/205628894-ed445a14-203a-4fe0-a603-93bcd1a2f9b4.png)

Add a new **.NET class library** to the solution and name it `CarRentalSystem.Infrastructure`. Reference the `Domain` project, and install these packages from NuGet:
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools

Afterwards, add a folder **Persistence** at the root of the project. Create **CarRentalDbContext**, define database sets for every domain entity, and set the model builder to search for configurations in the current assembly. Make sure the `CarRentalDbContext` class is marked as `internal`. It is a persistence detail, and it should not be visible to any other layer. Now, add a `Configurations` folder in the `Persistence` one and start creating database configurations for each domain model.

![image](https://user-images.githubusercontent.com/34960418/220346890-b77edcc4-e4d9-4a2d-835f-571a2a33e8e7.png)

Install the `Microsoft.EntityFrameworkCore.Design` NuGet package in the `CarRentalSystem.Startup` project. Then reference the Infrastructure project.

**Entity Framework Core** wants constructors that bind non-navigational properties, but according to the Domain-Driven Design principles, entities cannot be created with an invalid state. The solution is to **add additional private constructors** to our domain model classes for Entity Framework Core to use.

Open the Package Manager Console, choose the Infrastructure project, and add our first migration by calling:

```powershell
Add-Migration InitialDomainTables -OutputDir "Persistence/Migrations"
Update-Database
```

Check the database, the created schema and the database diagram.

![image](https://user-images.githubusercontent.com/34960418/220361375-6e8ffd7f-0ebf-4fa6-bc48-057a6770a930.png)


## The Application Layer and Repositories

Add a new .NET class library to the solution and name it `CarRentalSystem.Application`. Make sure it reference the `Domain` project. Create a `Contracts` folder in the `Application` project. Add the following `IRepository` interface

```csharp
public interface IRepository<out TEntity>
    where TEntity : IAggregateRoot
{
    IQueryable<TEntity> All();

    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}
```

The purpose of this abstraction is to restrict the access of the non-domain layers to aggregate roots only. It serves as an "anti-corruption" layer to our domain. Now, go to the `Infrastructure` project and add a `Repositories` folder in the `Persistence` one. Create the following `DataRepository` class.

```csharp
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
```

Then go to the `InfrastructureConfiguration` class and register the repository in the service provider. Finally, go to the `Web` project, and update the `CarAdsController` to use the repository.

```csharp
[ApiController]
[Route("[controller]")]
public class CarAdsController : ControllerBase
{
    private readonly IRepository<CarAd> carAds;

    public CarAdsController(IRepository<CarAd> carAds)
    {
        this.carAds = carAds;
    }

    /// <summary>
    /// Returns All Available Car Ads
    /// </summary>
    [HttpGet]
    public IEnumerable<CarAd> Get() => this.carAds
        .All()
        .Where(c => c.IsAvailable);
}
```

Check the Swagger again:

![image](https://user-images.githubusercontent.com/34960418/220375158-c0f194f3-5712-4bf8-beed-5c080080f39a.png)
