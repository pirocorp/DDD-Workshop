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

> Note
> 
> Ultimately, the Startup project will register all services from all layers.
> 
> ```csharp
>     private static void ConfigureServices(IServiceCollection services)
>        => services
>            .AddDomainServices()
>            .AddApplicationServices(configuration)
>            .AddInfrastructureServices(configuration)
>            .AddWebServices();
> ```


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

> **_NOTE:_**
> If **Domain Entities** are not suitable to be used as **Data Entities**, we can create **Data Entities** in **Persistence > Entities** folder in the **Infrastructure** project. These objects will inherit from **Domain Objects**. And in them, we can add all necessary foreign keys/relations, mapping tables, etc.
> The **DbContext** will use the **Data Entities** objects instead **Domain Entities**, but repositories will use **Domain Objects**. In other words, only the **Infrastructure** project(layer) will know that database uses **Data Entities**.
> And when we use the **DbContext** and return the **Data Entities** objects through the polymorphism, we will use the **Domain Objects** without any problem.
>
> Example:
> ```csharp
> public class MappingTable
> {
>     public Guid DealerId { get; set; }
>
>     public Dealer Dealer { get; set; }
>
>     public Guid CarAdId { get; set; }
>
>     public CarAd CarAd { get; set; }
> }
> ```
> 
> And in **Insfrastructure** project in **Persistence > Configurations** folder create **MappingTableConfiguration**.
>
>```csharp
> internal class MappingTableConfiguration : IEntityTypeConfiguration<MappingTable>
> {
>     public void Configure(EntityTypeBuilder<MappingTable> builder)
>     {
>         builder
>             .HasKey(e => new { e.DealerId, e.CarAdId });
>         // If there are no foreign keys declared in `MappingTable` (DDD-oriented style)
>         //  .HasKey("DealerId", "CarAdId");
>         
>         builder
>             .HasOne(e => e.CarAd)
>             .WithMany()
>             .HasForeignKey(e => e.CarAdId)
>         // If there are no foreign keys declared in `MappingTable` (DDD-oriented style)
>         //  .HasForeignKey("CarAdId");
>             .OnDelete(DeleteBehavior.Restrict);
>
>         builder
>             .HasOne(e => e.Dealer)
>             .WithMany()
>             .HasForeignKey(e => e.DealerId)
>         // If there are no foreign keys declared in `MappingTable` (DDD-oriented style)
>         //  .HasForeignKey("DealerId");
>             .OnDelete(DeleteBehavior.Restrict);
>     }
> }
> ```
>
> And don't forget to add `public DbSet<MappingTable> MappingTable => this.Set<MappingTable>();` in `CarRentalDbContext`.
>
> Another Example:
>```csharp
> public class CarAdDataEntity : CarAd
> {
>     public CarAd(
>         Manufacturer manufacturer, 
>         string model, 
>         Category category, 
>         string imageUrl,
>         decimal pricePerDay, 
>         Options options, 
>         bool isAvailable)
>         : base(manufacturer, model, category, imageUrl, pricePerDay, options, isAvailable)
>     {
>     }
>
>     // EF Core Only Constructor
>     private CarAd(
>         string model, 
>         string imageUrl,
>         decimal pricePerDay, 
>         bool isAvailable)
>         : base(null!, model, null!, imageUrl, pricePerDay, null!, isAvailable)
>     {
>     }
>
>     public Guid DealerId { get; private set; }
> }
>```
>
> Make a DbSet with the **Data Entity** `public DbSet<CarAdDataEntity> CarAds => this.Set<CarAdDataEntity>();` in `CarRentalDbContext`.
>
>```csharp
> public class CarAdRepository
> {
>     private readonly CarRentalDbContext dbContext;
>
>     internal CarAdRepository(CarRentalDbContext dbContext)
>     {
>         this.dbContext = dbContext;
>     }
>
>     public CarAd GetById(Guid id)
>     {
>         var carAd = this.dbContext.CarAds.Find(id);
>             
>         return carAd;
>     }
>
>     public IEnumerable<CarAd> GetCarsByDealerId(Guid dealerId)
>     {
>         return this.dbContext
>             .CarAds
>             .Where(c => c.DealerId == dealerId)
>             .ToList();
>     }
> }
>```
>


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


## Authentication with Identity

Install `Microsoft.AspNetCore.Identity.EntityFrameworkCore` into the **Infrastructure** project and create a folder named `Identity`. Add a `User` class in it:

```csharp
public sealed class User : IdentityUser
{
    internal User(string email)
        : base(email)
    {
        this.Email = email;
    }

    public Dealer? Dealer { get; private set; }

    public void BecomeDealer(Dealer dealer)
    {
        if (this.Dealer is not null)
        {
            throw new InvalidOperationException($"User '{this.UserName}' is already a dealer.");
        }

        this.Dealer = dealer;
    }
}
```

As you can see from the defined constructor - we try to follow the **best DDD practices** in the **User** class. To wire the **User** class to the database, we need to change the **DbContext** and add a user **configuration class**. 

- Make the **DbContext** inherit from **IdentityDbContext\<User\>**
- Add a `UserConfiguration` class in the **Persistence** \> **Configurations** folder

Open the Package Manager Console and create a new migration

```powershell
Add-Migration UserTable -OutputDir "Persistence/Migrations"
```

Implement automatic migrations in our code for better convenience. Run the application and validate that the database is now migrated.

Go to the **Application** project and create a custom `Result` type. This class is used in the application layer to return either a successful result or an error message. The successful `Result` may or may not have additional data.

```csharp
public class Result
{
    private readonly List<string> errors;

    internal Result(bool succeeded, List<string> errors)
    {
        this.Succeeded = succeeded;
        this.errors = errors;
    }

    public bool Succeeded { get; }

    public List<string> Errors
        => this.Succeeded
            ? new List<string>()
            : this.errors;

    public static Result Success 
        => new (true, new List<string>());

    public static Result Failure(IEnumerable<string> errors) 
        => new (false, errors.ToList());

    public static implicit operator Result(string error)
        => Failure(new List<string> { error });

    public static implicit operator Result(List<string> errors)
        => Failure(errors.ToList());

    public static implicit operator Result(bool success)
        => success ? Success : Failure(new[] { "Unsuccessful operation." });

    public static implicit operator bool(Result result)
        => result.Succeeded;
}

public class Result<TData> : Result
{
    private readonly TData data;

    private Result(bool succeeded, TData data, List<string> errors)
        : base(succeeded, errors)
        => this.data = data;

    public TData Data
        => this.Succeeded
            ? this.data
            : throw new InvalidOperationException(
                $"{nameof(this.Data)} is not available with a failed result. Use {this.Errors} instead.");

    public static Result<TData> SuccessWith(TData data) 
        => new (true, data, new List<string>());

    public new static Result<TData> Failure(IEnumerable<string> errors) 
        => new (false, default!, errors.ToList());

    public static implicit operator Result<TData>(string error)
        => Failure(new List<string> { error });

    public static implicit operator Result<TData>(List<string> errors)
        => Failure(errors);

    public static implicit operator Result<TData>(TData data)
        => SuccessWith(data);

    public static implicit operator bool(Result<TData> result)
        => result.Succeeded;
}
```

In the **Application** project add `ApplicationSettings`:

```csharp
public class ApplicationSettings
{
    public ApplicationSettings()
    {
        this.Secret = string.Empty;
    }

    public string Secret { get; private set; }
}
```

Install `Microsoft.Extensions.Options.ConfigurationExtensions` into the `Application` project and add `ApplicationConfiguration` to it:

```csharp
public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .Configure<ApplicationSettings>(
                configuration.GetSection(nameof(ApplicationSettings)),
                options => options.BindNonPublicProperties = true);
}
```

Now go to the **Program** class and update the **ConfigureServices** method to add the application services. Then open the appsettings.json file, and add the new section:

```json
"ApplicationSettings": {
    "Secret": "932edae7-f3e8-4ded-86d7-6f87075d571f"
},
```

You can validate that the application settings are configured correctly by using the **CarAdsController**.

```csharp
[ApiController]
[Route("[controller]")]
public class CarAdsController : ControllerBase
{
    private readonly IRepository<CarAd> carAds;
    private readonly IOptions<ApplicationSettings> settings;

    public CarAdsController(
        IRepository<CarAd> carAds, 
        IOptions<ApplicationSettings> settings)
    {
        this.carAds = carAds;
        this.settings = settings;
    }

    /// <summary>
    /// Returns All Available Car Ads
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public object Get() => new
    {
        Settings = this.settings,
        CarAds = this.carAds
            .All()
            .Where(c => c.IsAvailable),
    };
}
```

![image](https://user-images.githubusercontent.com/34960418/220917272-e7b50fd1-6e92-42c2-ad9e-63f27ed29837.png)


If everything is working correctly, then go to the **Application** project and add a folder named `Features`. Create another folder inside of it – `Identity`. Add these two classes there:

```csharp
public class UserInputModel
{
    public UserInputModel(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }

    public string Email { get; }

    public string Password { get; }
}

public class LoginOutputModel
{
    public LoginOutputModel(string token)
    {
        this.Token = token;
    }

    public string Token { get; }
}
```

Afterwards, create an IIdentity interface in the Contracts folder:

```csharp
public interface IIdentity
{
    Task<Result> Register(UserInputModel userInput);

    Task<Result<LoginOutputModel>> Login(UserInputModel userInput);
}
```

Your application project should look like this:

![image](https://user-images.githubusercontent.com/34960418/220920494-e0714793-ad77-4ce8-bedb-e5bba13ebeee.png)

The `IIdentity` interface will be implemented by the **Infrastructure** layer. Go to it and install `Microsoft.AspNetCore.Authentication.JwtBearer` from NuGet. Create the `IdentityService` file in Infrastructure > Identity folder.

```csharp
internal class IdentityService : IIdentity
{
    private const string InvalidLoginErrorMessage = "Invalid credentials.";

    private readonly UserManager<User> userManager;
    private readonly ApplicationSettings applicationSettings;

    public IdentityService(
        UserManager<User> userManager, 
        IOptions<ApplicationSettings> applicationSettings)
    {
        this.userManager = userManager;
        this.applicationSettings = applicationSettings.Value;
    }

    public async Task<Result> Register(UserInputModel userInput)
    {
        var user = new User(userInput.Email);

        var identityResult = await this.userManager.CreateAsync(user, userInput.Password);

        var errors = identityResult.Errors
            .Select(e => e.Description);

        return identityResult.Succeeded
            ? Result.Success
            : Result.Failure(errors);
    }

    public async Task<Result<LoginOutputModel>> Login(UserInputModel userInput)
    {
        var user = await this.userManager.FindByEmailAsync(userInput.Email);
        if (user == null)
        {
            return InvalidLoginErrorMessage;
        }

        var passwordValid = await this.userManager.CheckPasswordAsync(user, userInput.Password);
        if (!passwordValid)
        {
            return InvalidLoginErrorMessage;
        }

        var token = this.GenerateJwtToken(
            user.Id,
            user.Email ?? string.Empty);

        return new LoginOutputModel(token);
    }

    private string GenerateJwtToken(string userId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(this.applicationSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var encryptedToken = tokenHandler.WriteToken(token);

        return encryptedToken;
    }
}
```

In `InfrastructureConfiguration` configure the Identity System and JWT. Create `IdentityController` file in the Web > Features folder. Configure Swagger to use JWT.

```csharp
[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentity identity;

    public IdentityController(IIdentity identity)
    {
        this.identity = identity;
    }

    [HttpPost]
    [Route(nameof(Register))]
    public async Task<ActionResult> Register(UserInputModel model)
    {
        var result = await this.identity.Register(model);

        if (!result.Succeeded)
        {
            return this.BadRequest(result.Errors);
        }

        return this.Ok();
    }

    [HttpPost]
    [Route(nameof(Login))]
    public async Task<ActionResult<LoginOutputModel>> Login(UserInputModel model)
    {
        var result = await this.identity.Login(model);

        if (!result.Succeeded)
        {
            return this.BadRequest(result.Errors);
        }

        return result.Data;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return this.Ok(this.User.Identity?.Name);
    }
}
```

## Creating Entities with Builder Factories

Go to the **Domain** project and make sure every **entity** has an **internal constructor**, even the **aggregate roots**. Add a factory layer which will be responsible for instantiating valid objects. Constructors are excellent for this purpose, but since **entities** have a lot of properties, the Builder pattern will be more suitable and convenient from a developer's point of view.

Create a folder Factories in the Domain project, and add an IFactory interface in it:

```csharp
public interface IFactory<out TEntity> 
    where TEntity : IAggregateRoot
{
    TEntity Build();
}
```

As you can see, factories allow only aggregate roots. Now, add the following structure:

![image](https://user-images.githubusercontent.com/34960418/221152019-22302cbb-5eef-4bdb-828e-0fd2b5cfcf72.png)

This is the `IDealerFactory` interface:

```csharp
public interface IDealerFactory : IFactory<Dealer>
{
    IDealerFactory WithName(string name);

    IDealerFactory WithPhoneNumber(string phoneNumber);
}
```

And this is its implementation:

```csharp
public class DealerFactory : IDealerFactory
{
    private string dealerName = string.Empty;

    private string dealerPhoneNumber = string.Empty;

    public Dealer Build() => new (this.dealerName, this.dealerPhoneNumber);

    public IDealerFactory WithName(string name)
    {
        this.dealerName = name;

        return this;
    }

    public IDealerFactory WithPhoneNumber(string phoneNumber)
    {
        this.dealerPhoneNumber = phoneNumber;

        return this;
    }
}
```

Builder factories save us time from writing too many (and too long) constructors, but they do not have compile-time type safety. 

Install the `Microsoft.Extensions.DependencyInjection` and `Scrutor` packages to the **Domain** project. [Scrutor](https://github.com/khellang/Scrutor) add assembly scanning capabilities to the ASP.NET Core DI container. Scrutor is not a dependency injection (DI) container itself, instead it adds additional capabilities to the built-in container.

```csharp
  public static IServiceCollection AddDomainServices(this IServiceCollection services)
      => services
          .Scan(scan => scan
              .FromCallingAssembly()
              .AddClasses(classes => classes.AssignableTo(typeof(IFactory<>)))
              .AsMatchingInterface()
              .WithTransientLifetime());
```

Using **Scrutor** to register factories automatically. Just call the **AddDomainServices** method in the **Program** file. Write unit tests for the factory classes. Validate that the **factories** cannot create an **aggregate** without its related **entities**. And add a unit test that factories are registered in the DI container.


## Simplifying Business Logic with CQRS and MediatR

Separate the business logic with **CQRS**. **Commands** are business rules which change the state of the application, and **queries** just fetch information without mutating any data. Install `MediatR` to the **Application** project. Then configure **MediatR** in the `ApplicationConfiguration`:

```csharp
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration? configuration)
        => services
            .Configure<ApplicationSettings>(
                configuration?.GetSection(nameof(ApplicationSettings))
                ?? throw new InvalidOperationException($"Missing {nameof(ApplicationSettings)}"),
                options => options.BindNonPublicProperties = true)
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
```

Create `ApiController` in root of the **Web** project. All controllers should now inherit from the `ApiController`.

```csharp
[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
    private IMediator? mediator;

    protected IMediator Mediator
        => this.mediator 
            ??= this.HttpContext.RequestServices.GetService<IMediator>() 
            ?? throw new InvalidOperationException("IMediator service is not registered in the DI container.");
}
```

### Create Login User Command

Go to the Application project and create a `LoginUserCommand` class and the folder structure shown below. Move the `LoginOutputModel` too. Make sure you fix its namespace.

![image](https://user-images.githubusercontent.com/34960418/221203049-38ca81bb-a928-45b0-b0c9-b86a0ac22f7d.png)

Open the `LoginUserCommand` and inherit the `UserInputModel` and its constructor because the properties are the same. Add an inner class `LoginUserCommandHandler` like so:

```csharp
public class LoginUserCommand : UserInputModel, IRequest<Result<LoginOutputModel>>
{
    public LoginUserCommand(string email, string password) 
        : base(email, password)
    { }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginOutputModel>>
    {
        private readonly IIdentity identity;

        public LoginUserCommandHandler(IIdentity identity)
        {
            this.identity = identity;
        }

        public async Task<Result<LoginOutputModel>> Handle(
            LoginUserCommand request,
            CancellationToken cancellationToken)
            => await this.identity.Login(request);
    }
}
```

Go to the **Web** project and create a folder **Common**. Create `ResultExtensions` file. This class provides friendly extensions methods to convert objects into HTTP action results.

```csharp
public static class ResultExtensions
{
    public static async Task<ActionResult<TData>> ToActionResult<TData>(this Task<TData> resultTask)
    {
        var result = await resultTask;

        if (result == null)
        {
            return new NotFoundResult();
        }

        return result;
    }

    public static async Task<ActionResult> ToActionResult(this Task<Result> resultTask)
    {
        var result = await resultTask;

        if (!result.Succeeded)
        {
            return new BadRequestObjectResult(result.Errors);
        }
            
        return new OkResult();
    }

    public static async Task<ActionResult<TData>> ToActionResult<TData>(this Task<Result<TData>> resultTask)
    {
        var result = await resultTask;

        if (!result.Succeeded)
        {
            return new BadRequestObjectResult(result.Errors);
        }

        return result.Data;
    }
}
```

Make the Login method in the `IdentityController` work with the new command:

```csharp
[HttpPost]
[Route(nameof(Login))]
public async Task<ActionResult<LoginOutputModel>> Login(LoginUserCommand command)
    => await this.Mediator.Send(command).ToActionResult();
```

Here are the rules you need to follow with **CQRS** in the application layer:
  - Add a **command** or **query** class (**the input model**) containing the request properties. We will cover validation in one of the next sections.
  - Implement the `IRequest<TResponse>` interface.
  - `TResponse` should be:
    - `Result` if the possible responses are either success with no data or error messages.
    - `Result<TOutputModel>` if the possible responses are either success with specific response data or error messages.
    - An `OutputModel` if you do not need to handle error messages in the business logic.
  - **Input** and **output** models should only define properties which the particular use cases require.
  - **Input** or **output** models should **not inherit or reference** any **domain models**.
  - **Input** or output **models** should be encapsulated and serializable.
  - Aim to have **private setters** in your models.
  - **Do not use the same input or output model for multiple scenarios**. You may extract base classes for code reuse.
  - **Do not use the same model for both input and output scenarios**.
  - Create an inner handler class for each **command** and query and **implement** `IRequestHandler<TRequest, TResponse>`. The `TRequest` should be the **input model** class.
  - **Inject in the handler class all the services you may need** – repositories, factories, etc. Return the expected `TResponse` result.

If you follow these rules, it will be easy to **separate the HTTP logic from the business one**. The **Controller** should always delegate the request to inner services. Its single responsibility should be binding the request data and producing an action result.


### Create a query for searching the car ads in the system

#### Define Query Response Data Format

Add files and folders to match the following structure:

![image](https://user-images.githubusercontent.com/34960418/221847933-b2b6e061-288c-4088-aaab-de856122e342.png)

Response data format:

```csharp
public class CarAdListingModel
{
    internal CarAdListingModel(
        Guid id, 
        string manufacturer,
        string model,
        string imageUrl,
        string category, 
        decimal pricePerDay)
    {
        this.Id = id;
        this.Manufacturer = manufacturer;
        this.Model = model;
        this.ImageUrl = imageUrl;
        this.Category = category;
        this.PricePerDay = pricePerDay;
    }

    public Guid Id { get; }

    public string Manufacturer { get; }

    public string Model { get; }

    public string ImageUrl { get; }

    public string Category { get; }

    public decimal PricePerDay { get; }
}

public class SearchCarAdsOutputModel
{
    internal SearchCarAdsOutputModel(IEnumerable<CarAdListingModel> carAds, int total)
    {
        this.CarAds = carAds;
        this.Total = total;
    }

    public IEnumerable<CarAdListingModel> CarAds { get; }

    public int Total { get; }
}
```

#### Create and configure `CarAdRepository`

Create `ICarAdRepository` in **Application** > **Features** > **CarAds**.

The repositories’ sole purpose is to call the persistence layer and query the database. Do not put domain or application logic in them. They should be marked as internal, and their registration is done automatically by scanning the assembly. A repository’s interface should not return `IQueryable` collections directly. Use `IEnumerable`. Domain entities are allowed for both input or output. Make sure that base repository is following these rules.

```csharp
public interface ICarAdRepository : IRepository<CarAd>
{
    Task<IEnumerable<CarAdListingModel>> GetCarAdListings(
        string? manufacturer = default,
        CancellationToken cancellationToken = default);

    Task<int> Total(CancellationToken cancellationToken = default);
}
```

Remember – the sole purpose of the base repository interface is to provide a markup interface for our aggregate roots.

```csharp
public interface IRepository<out TEntity> where TEntity : IAggregateRoot
{ }
```

Implement `CarAdRepository` in **Infrastructure** > **Persistence** > **Repositories**.

```csharp
internal class CarAdRepository : DataRepository<CarAd>, ICarAdRepository
{
    public CarAdRepository(CarRentalDbContext database) 
        : base(database)
    { }

    public async Task<IEnumerable<CarAdListingModel>> GetCarAdListings(
        string? manufacturer = default,
        CancellationToken cancellationToken = default)
    {
        var query = this.AllAvailable();

        if (!string.IsNullOrWhiteSpace(manufacturer))
        {
            query = query
                .Where(car => EF
                    .Functions
                    .Like(car.Manufacturer.Name, $"%{manufacturer}%"));
        }

        return await query
            .Select(car => new CarAdListingModel(
                car.Id,
                car.Manufacturer.Name,
                car.Model,
                car.ImageUrl,
                car.Category.Name,
                car.PricePerDay))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> Total(CancellationToken cancellationToken = default)
        => await this
            .AllAvailable()
            .CountAsync(cancellationToken);

    private IQueryable<CarAd> AllAvailable()
        => this
            .All()
            .Where(car => car.IsAvailable);
}
```

Make sure that `DataRepository` is `internal` and `abstract`

```csharp
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
```

Configure repositories registration to be done automatically by scanning the assembly. In `InfrastructureConfiguration` file in **Infrastructure** project add method `AddRepositories` which will scan the saaembly and register all repositories and call this method in `AddInfrastructureServices` public method.

```csharp
public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    IConfiguration? configuration)
        => services
            .AddDatabase(configuration)
            .AddRepositories()
            .AddIdentity(configuration)
            .AddSwagger();

private static IServiceCollection AddRepositories(this IServiceCollection services)
    => services
        .Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes
                .AssignableTo(typeof(IRepository<>)))
            .AsMatchingInterface()
            .WithTransientLifetime());
```

Create a unit test that ensures that Repositories are registered.

```csharp
[Fact]
public void AddRepositoriesShouldRegisterRepositories()
{
    // Arrange
    var serviceCollection = new ServiceCollection()
        .AddDbContext<CarRentalDbContext>(
            opts 
                => opts.UseInMemoryDatabase(Guid.NewGuid().ToString()));

    var method = typeof(InfrastructureConfiguration)
        .GetMethod("AddRepositories", BindingFlags.Static | BindingFlags.NonPublic);

    var parameters = new object[] { serviceCollection };

    // Act
    var services = ((IServiceCollection?) method
            ?.Invoke(serviceCollection, parameters) 
            ?? throw new InvalidOperationException($"AddRepositories method in {nameof(InfrastructureConfiguration)} not found"))
        .BuildServiceProvider();

    // Assert
    services
        .GetService<ICarAdRepository>()
        .Should()
        .NotBeNull();
}
```

Your folder structure should look like the following:

![image](https://user-images.githubusercontent.com/34960418/221868227-b68f68a1-ccbc-473a-a8aa-927ea206fb2f.png)


#### Implement `SearchCarAdsQuery`

It should be implemented using the same principles as the command we wrote earlier. There is a small difference though. Since queries contain optional data – there is no need to encapsulate the properties behind a constructor.

The `Manufacturer` property should be a nullable string because it is optional in our logic. Its setter is public, because otherwise the built-in complex type model binder will not be able to transform the GET request.

```csharp
public class SearchCarAdsQuery : IRequest<SearchCarAdsOutputModel>
{
    public string? Manufacturer { get; set; }

    public class SearchCarAdsQueryHandler : IRequestHandler<SearchCarAdsQuery, SearchCarAdsOutputModel>
    {
        private readonly ICarAdRepository carAdRepository;

        public SearchCarAdsQueryHandler(ICarAdRepository carAdRepository)
        {
            this.carAdRepository = carAdRepository;
        }

        public async Task<SearchCarAdsOutputModel> Handle(
            SearchCarAdsQuery request, 
            CancellationToken cancellationToken)
        {
            var carAdListings = await this.carAdRepository.GetCarAdListings(
                request.Manufacturer,
                cancellationToken);

            var totalCarAds = await this.carAdRepository.Total(cancellationToken);

            return new SearchCarAdsOutputModel(carAdListings, totalCarAds);
        }
    }
}
```

And `CarAdsController` which will use MediatR to "Send" the query

```csharp
public class CarAdsController : ApiController
{
    [HttpGet]
    public async Task<ActionResult<SearchCarAdsOutputModel>>Get(
        [FromQuery] SearchCarAdsQuery query) 
        => await this.Mediator.Send(query);
}
```

## Integration Tests of Web Features

First create `CarRentalSystem.Fakes` library in test folder, install `FakeItEasy`, `FakeItEasy.Analyzer.CSharp` and `Bogus` from **NuGet**. `Bogus` is a library which allows us to create random fake data. Create `CarAd.Fakes`, `Category.Fakes`, `Manufacturer.Fakes` and `Options.Fakes`

```csharp
public class CarAdFakes
{
    public class CarAdDummyFactory : DummyFactory<CarAd>
    {
        protected override CarAd Create() => Data.GetCarAd();
    }

    public static class Data
    {
        public static IEnumerable<CarAd> GetCarAds(int count = 10)
            => Enumerable
                .Range(1, count)
                .Select(i => GetCarAd(i))
                .Concat(Enumerable
                    .Range(count + 1, count * 2)
                    .Select(i => GetCarAd(i, false)))
                .ToList();

        public static CarAd GetCarAd(int id = 1, bool isAvailable = true)
            => new Faker<CarAd>()
                .CustomInstantiator(f => new CarAd(
                    new Manufacturer($"Manufacturer {id}"),
                    f.Lorem.Letter(10),
                    A.Dummy<Category>(), 
                    f.Image.PicsumUrl(),
                    f.Random.Number(100, 200),
                    A.Dummy<Options>(), 
                    isAvailable))
                .Generate();
    }
}
```

Create `CarRentalSystem.Web.Tests` xUnit project and add references to `CarRentalSystem.Startup` and `CarRentalSystem.Domain.Fakes`. Add the `CustomWebApplicationFactory` class to the `CarRentalSystem.Web.Tests` project. This class will change the **DbContext** configuration to use a test database instead.

```csharp
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> 
    where TProgram : class
{
    private readonly string databaseId;

    public CustomWebApplicationFactory(string databaseId)
    {
        this.databaseId = databaseId;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.Single(
                d => d.ServiceType ==
                     typeof(DbContextOptions<CarRentalDbContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbConnection));

            services.Remove(dbConnectionDescriptor!);

            // Calling Migrate on InMemory database throws exception
            var migrationDescriptor = services.SingleOrDefault(
                d => d.ImplementationType == typeof(CarRentalDbInitializer));

            services.Remove(migrationDescriptor!);

            services.AddDbContext<CarRentalDbContext>((container, options) =>
            {
                options.UseInMemoryDatabase(this.databaseId);
                //options.UseSqlServer("Server=PIRO\\SQLEXPRESS2019;Database=DDD-Workshop-IntegrationTests;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True");
            });
        });

        builder.UseEnvironment("Development");
    }
}
```

Add the `CarAdsControllerTests` class and write the test for the Get method. (For now, System.Json cannot deserialize into types with no public constructors, so use Newtonsoft.Json to deserialize into record types)

```csharp
public class CarAdsControllerTests
{
    private readonly CustomWebApplicationFactory<Program> webFactory;
    private readonly HttpClient httpClient;

    public CarAdsControllerTests()
    {
        this.webFactory = new CustomWebApplicationFactory<Program>(Guid.NewGuid().ToString());
        this.httpClient = this.webFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task CarAdsControllerGetReturnsEmptyArrayWhenNoAdsAreFound()
    {
        // Act
        var response = await this.httpClient.GetAsync("/CarAds");
        var stringResult = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<GetResult>(stringResult);

        // Assert
        Assert.NotNull(actual);
        Assert.Empty(actual.CarAds);
        actual.Total.Should().Be(0);
    }

    [Fact]
    public async Task CarAdsControllerGetReturnsCollectionOfCarAds()
    {
        // Arrange
        await this.SeedCarAds(10);

        // Act
        var response = await this.httpClient.GetAsync("/CarAds");
        var stringResult = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<GetResult>(stringResult);

        // Assert
        actual.Should().NotBeNull();
        actual!.CarAds.Should().NotBeEmpty();
        actual!.CarAds.Count().Should().Be(10);
        actual!.Total.Should().Be(10);
    }

    [Fact]
    public async Task CarAdsControllerGetReturnsCorrectAdsByManufacturer()
    {
        // Arrange
        await this.SeedCarAds(10);

        // Act
        var response = await this.httpClient.GetAsync("/CarAds?manufacturer=Manufacturer%202");
        var stringResult = await response.Content.ReadAsStringAsync();
        var actual = JsonConvert.DeserializeObject<GetResult>(stringResult);

        // Assert
        actual.Should().NotBeNull();
        actual!.CarAds.Should().NotBeEmpty();
        actual!.CarAds.Count().Should().Be(1);
        actual!.Total.Should().Be(10);
    }

    private async Task SeedCarAds(int count)
    {
        var ads = CarAdFakes.Data.GetCarAds(count);

        await using var scope = this.webFactory.Services.CreateAsyncScope();
        var database = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();

        await database.AddRangeAsync(ads);
        await database.SaveChangesAsync();
    }

    private record GetResult(IEnumerable<CarAdListingModel> CarAds, int Total);
}
```

Create `IdentityServiceFakes` and `JwtTokenGeneratorServiceFakes` in `CarRentalSystem.Fakes` project. Install `Microsoft.AspNetCore.Mvc.Testing` and `Microsoft.EntityFrameworkCore.InMemory` packets from **NuGet** in the same project. Create `WebApplicationFactoryWithFakeUserManager` which uses fake objects for `UserManager<User>` and `IJwtTokenGenerator`.

```csharp
public class WebApplicationFactoryWithFakeUserManager<TProgram> : CustomWebApplicationFactory<TProgram>
    where TProgram : class
{
    public WebApplicationFactoryWithFakeUserManager(string databaseId) 
        : base(databaseId)
    { }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            var userManagerDescriptor = services.Single(
                d => d.ImplementationType == typeof(UserManager<User>));

            services.Remove(userManagerDescriptor);

            var jwtTokenGeneratorDescriptor = services.Single(
                d => d.ServiceType == typeof(IJwtTokenGenerator));

            services.Remove(jwtTokenGeneratorDescriptor);

            services.AddTransient(_ => IdentityFakes.FakeUserManager);
            services.AddTransient(_ => JwtTokenGeneratorFakes.FakeJwtTokenGenerator);
        });
    }
}
```

Create tests for Login functionality with `WebApplicationFactoryWithFakeUserManager` which uses fake objects for `UserManager<User>` and `IJwtTokenGenerator`.

```csharp
public class IdentityControllerTests
{
    private WebApplicationFactory<Program> webFactory;
    private HttpClient httpClient;

    public IdentityControllerTests()
    {
        this.webFactory = new WebApplicationFactoryWithFakeUserManager<Program>(Guid.NewGuid().ToString());
        this.httpClient = this.webFactory.CreateDefaultClient();
    }

    [Theory]
    [InlineData(
        IdentityFakes.TEST_EMAIL,
        IdentityFakes.VALID_PASSWORD,
        JwtTokenGeneratorFakes.VALID_TOKEN)]
    public async Task LoginShouldReturnToken(string email, string password, string token)
    {
        // Arrange
        var payload = JsonConvert.SerializeObject(new
        {
            Email = email,
            Password = password
        });

        // Act
        var response = await this.PostToEndpoint("/Identity/Login", payload);
        var content = JsonConvert.DeserializeObject<TokenResult>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content!.Token.Should().Be(token);
    }

    [Fact]
    public async Task LoginShouldReturn400BadRequestWithInvalidCredentials()
    {
        // Arrange
        var payload = JsonConvert.SerializeObject(new
        {
            Email = "invalid@example.com",
            Password = "invalid"
        });

        // Act
        var response = await this.PostToEndpoint("/Identity/Login", payload);
        var errors = JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());

        errors.Should().NotBeEmpty();
    }
    
    private async Task<HttpResponseMessage> PostToEndpoint(string endpoint, string payload)
        => await this.httpClient.PostAsync(
            endpoint,
            new StringContent(payload, Encoding.UTF8, "application/json"));

    private record TokenResult(string Token);
}
```

## Creating Entities

As per our requirements – all registered users should be dealers. And all dealers should be able to add car ads.

Go to **Application > Features > Identity** and create an IUser interface. We need to expose the User object in our commands:

```csharp
public interface IUser
{
    void BecomeDealer(Dealer dealer);
}
```

Our User class should implement the above interface. Go to **Infrastructure > Identity** and make the **IdentityService**'s `Register` method return a `Result<IUser>`. Fix the IIdentity interface too.

```csharp
public async Task<Result<IUser>> Register(UserInputModel userInput)
{
    var user = new User(userInput.Email);

    var identityResult = await this.userManager.CreateAsync(user, userInput.Password);

    var errors = identityResult.Errors.Select(e => e.Description);

    return identityResult.Succeeded
        ? Result<IUser>.SuccessWith(user)
        : Result<IUser>.Failure(errors);
}
```

Create a **Dealers** folder in **Application > Features**. Add IDealerRepository in it. Implement the following interface in **Infrastructure > Persistence > Repositories**

```csharp
public interface IDealerRepository : IRepository<Dealer>
{
    Task Save(Dealer dealer, CancellationToken cancellationToken = default);
}
```

Now go to the **Application > Features > Identity > Commands > CreateUser**. Update the **CreateUserCommand** to receive a name and a phone number. Update the handler’s logic. It should create a user, then create a dealer, then update the user and save it to the database.

```csharp
public class CreateUserCommand : UserInputModel, IRequest<Result>
{
    public CreateUserCommand(
        string email, 
        string password,
        string name,
        string phoneNumber) 
        : base(email, password)
    {
        this.Name = name;
        this.PhoneNumber = phoneNumber;
    }

    public string Name { get; }

    public string PhoneNumber { get; }     

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IIdentity identity;
        private readonly IDealerFactory dealerFactory;
        private readonly IDealerRepository dealerRepository;

        public CreateUserCommandHandler(
            IIdentity identity,
            IDealerFactory dealerFactory,
            IDealerRepository dealerRepository)
        {
            this.identity = identity;
            this.dealerFactory = dealerFactory;
            this.dealerRepository = dealerRepository;
        }

        public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result =  await this.identity.Register(command);

            if (!result.Succeeded)
            {
                return result;
            }

            var user = result.Data;

            var dealer = this.dealerFactory
                .WithName(command.Name)
                .WithPhoneNumber(command.PhoneNumber)
                .Build();

            user.BecomeDealer(dealer);
            await this.dealerRepository.Save(dealer, cancellationToken);

            return result;
        }
    }
}
```

Try to build the solution and fix any failing unit test. You may turn on the **Run Tests In Parallel** feature for better performance:

![image](https://user-images.githubusercontent.com/34960418/223690896-a8db4fbf-fa68-4158-97b0-7b9b8b79f384.png)

Car ad categories should be seeded to the database before the application is used by the end users. We are now going to add infrastructure for data seeding. Add an **IInitialData** interface to the **Domain > Common** folder.

```csharp
internal interface IInitialData
{
    Type EntityType { get; }

    IEnumerable<object> GetData();
}
```

Register the category initial data class in the DomainConfiguration:

```csharp
public static IServiceCollection AddDomainServices(this IServiceCollection services)
    => services
        .Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.AssignableTo(typeof(IFactory<>)))
            .AsMatchingInterface()
            .WithTransientLifetime())
        .AddTransient<IInitialData, CategoryData>();
```

Finally, replace the **CarRentalDbInitializer**. Running the application should populate the category data.

```csharp
internal class CarRentalDbInitializer : IInitializer
{
    private readonly CarRentalDbContext db;
    private readonly IEnumerable<IInitialData> initialDataProviders;

    public CarRentalDbInitializer(
        CarRentalDbContext db,
        IEnumerable<IInitialData> initialDataProviders)
    {
        this.db = db;
        this.initialDataProviders = initialDataProviders;
    }

    public void Initialize()
    {
        this.db.Database.Migrate();

        foreach (var initialDataProvider in this.initialDataProviders)
        {
            if (!this.DataSetIsEmpty(initialDataProvider.EntityType))
            {
                continue;
            }

            var data = initialDataProvider.GetData();
            this.db.AddRange(data);
        }

        this.db.SaveChanges();
    }

    private bool DataSetIsEmpty(Type type)
    {
        var setMethod = this.GetType()
            .GetMethod(nameof(this.GetSet), BindingFlags.Instance | BindingFlags.NonPublic)
            ?.MakeGenericMethod(type);

        var set = setMethod?.Invoke(this, Array.Empty<object>());

        var countMethod = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == nameof(Queryable.Count) && m.GetParameters().Length == 1)
            .MakeGenericMethod(type);

        var result = (int)countMethod.Invoke(null, new[] { set })!;

        return result is 0;
    }

    private DbSet<TEntity> GetSet<TEntity>()
        where TEntity : class
        => this.db.Set<TEntity>();
}
```

We can use the **CategoryData** class to add more domain logic in our **CarAd** for validating **Category**.

We will also need to know the current request user to create a car ad. Add the following interface to **Application > Contracs**:

```csharp
public interface ICurrentUser
{
    string UserId { get; }
}
```

The implementation of that interface should come from the web project since it is the only one familiar with request data. Create a folder Services and add **CurrentUserService** in it:

```csharp
public class CurrentUserService : ICurrentUser
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.UserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? throw new InvalidOperationException("This request does not have an authenticated user.");
    }

    public string UserId { get; }
}
```

Register **CurrentUserService** in **WebConfiguration**

```csharp
public static IServiceCollection AddWebServices(
    this IServiceCollection services)
{
    services.AddControllers();

    services.AddTransient<ICurrentUser, CurrentUserService>();

    return services;
}
```

You may notice the **Get** method is now renamed to **Search**. It is always a good idea to give meaningful action names, which are tightly connected to the business logic, and not the technology itself. In this case, **Search** is way better than **Get**.

```csharp
    [HttpGet]
    public async Task<ActionResult<SearchCarAdsOutputModel>>Search([FromQuery] SearchCarAdsQuery query) 
        => await this.Send(query);

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreateCarAdOutputModel>> Create(CreateCarAdCommand command)
        => await this.Send(command);
```

## Adding Validation

MediatR Pipeline Behaviour

MediatR Pipeline behaviours were introduced in Version 3, enabling you execute validation or logging logic before and after your Command or Query Handlers execute, resulting in your handlers only having to deal with Valid requests in your [CQRS implementation](https://garywoodfine.com/what-is-cqrs/), and you don't have to clutter your Handler methods with repetitive logging or validation logic!

More information for MediatR [Behaviours](https://garywoodfine.com/how-to-use-mediatr-pipeline-behaviours/)

Add a validation pipeline behaviour

One of the most common things you'll need to do when using MediatR is validation. Most likely you'd like to validate your Request and Responses to ensure that they have all the data in the correct format. The important aspect here, is that you don't want to pollute or clutter your handler methods with repetitive validation logic. This is just inevitably increase the [Cyclomatic complexity](https://en.wikipedia.org/wiki/Cyclomatic_complexity) of your methods, and as software developers we define complexity as anything related to the structure of a software system that makes it hard to understand and modify the system.

Pipeline Behaviours enable us to implement Separation of Concerns software design principle , which is an important software architecture that aims to ensure that code is separated into layers and components that each have distinct functionality with as little overlap as possible.

Install `FluentValidation.DependencyInjectionExtensions` to the **Web** project, and `FluentValidation` to the **Application** one. 

Create `ModelValidationException` and `NotFoundException` in Application > Exceptions.

```csharp
public class ModelValidationException : Exception
{
    public ModelValidationException()
        : base("One or more validation errors have occurred.")
    {
        this.Errors = new Dictionary<string, string[]>();
    }

    public ModelValidationException(IEnumerable<ValidationFailure> errors)
        : this()
    {
        var failureGroups = errors
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            this.Errors.Add(propertyName, propertyFailures);
        }
    }

    public IDictionary<string, string[]> Errors { get; }
}
```

Create `ValidationExceptionHandlerMiddleware` in **Web > Middleware**.

```csharp
public class ValidationExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ValidationExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        var result = string.Empty;

        switch (exception)
        {
            case ModelValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = SerializeObject(new
                {
                    ValidationDetails = true,
                    validationException.Errors
                });
                break;
            case NotFoundException _:
                code = HttpStatusCode.NotFound;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (string.IsNullOrEmpty(result))
        {
            result = SerializeObject(new[] { exception.Message });
        }

        return context.Response.WriteAsync(result);
    }

    private static string SerializeObject(object obj)
        => JsonSerializer.Serialize(obj, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        });
}

public static class ValidationExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<ValidationExceptionHandlerMiddleware>();
}
```

Create `RequestValidationBehavior` in **Application > Behaviours**. 

```csharp
public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var tasks = this
            .validators
            .Select(v => v.ValidateAsync(request, cancellationToken));

        var errors = (await Task.WhenAll(tasks))
            .SelectMany(v => v.Errors)
            .Where(f => f != null)
            .ToList();

        if (errors.Count != 0)
        {
            throw new ModelValidationException(errors);
        }

        return await next();
    }
}
```

Go to `ApplicationConfiguration` and register the `RequestValidationBehavior`:

```csharp
public static IServiceCollection AddApplicationServices(
    this IServiceCollection services,
    IConfiguration? configuration)
    => services
        .Configure<ApplicationSettings>(
            configuration?.GetSection(nameof(ApplicationSettings))
                ?? throw new InvalidOperationException($"Missing {nameof(ApplicationSettings)} configuration"),
            options => options.BindNonPublicProperties = true)
        .AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        })
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
```

Then go to `WebConfiguration` and register `FluentValidation`:

```csharp
public static IServiceCollection AddWebServices(
    this IServiceCollection services)
{
    services
        .AddScoped<ICurrentUser, CurrentUserService>()
        .AddValidatorsFromAssemblyContaining<Result>()
        .AddControllers();
}
```

Finally, update the `ConfigureMiddleware` method in the `Program` class to use the new middleware:

```csharp
private static void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseValidationExceptionHandler();
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.Initialize();
}
```

We did the following:

- We turned off the default validation and added **FluentValidation** to the application services (`options.SuppressModelStateInvalidFilter = true;`). 
- We added a pipeline behavior to **MediatR**. Every time we send a command or a query, it will execute the pipeline against that behavior. The latter will run all registered validators in the system against the request object (`RequestValidationBehavior`).
- If an error is found, the behavior will throw an exception, and the request pipeline will not continue(`if (errors.Count != 0) { throw new ModelValidationException(errors); }`).
- The exception-handling middleware will catch the exception and serialize a friendly error JSON object for the client to consume (`ValidationExceptionHandlerMiddleware`).

There are three types of error responses in our application:
  - An exception – we return its message as a collection of one error (`ValidationExceptionHandlerMiddleware` In `HandleExceptionAsync` method `result = SerializeObject(new[] { exception.Message });`).
  - A logic error – we return a **Result** or a **Result<T>** object with a collection of errors.
  - A validation error – we return a collection of properties with their errors. In this case, we add one additional property – **ValidationDetails** to indicate that we are returning a nested collection (`ValidationExceptionHandlerMiddleware` In `HandleExceptionAsync` method `case ModelValidationException validationException:`).

This way, our API returns error messages according to a convention and the client will have an easy for implementation validation handler. Since our validation infrastructure is ready, we can now add command validators.

Go to **Application > Features > Identity > Commands > CreateUser** and add a `CreateUserCommandValidator` class:

```csharp
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        this.RuleFor(u => u.Email)
            .MinimumLength(MIN_EMAIL_LENGTH)
            .MaximumLength(MAX_EMAIL_LENGTH)
            .EmailAddress()
            .NotEmpty();

        this.RuleFor(u => u.Password)
            .MaximumLength(MAX_NAME_LENGTH)
            .NotEmpty();

        this.RuleFor(u => u.Name)
            .MinimumLength(MIN_NAME_LENGTH)
            .MaximumLength(MAX_NAME_LENGTH)
            .NotEmpty();

        this.RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .Matches(PHONE_NUMBER_REGULAR_EXPRESSION);
    }
}
```

With a separate validator class, we can easily implement advanced logic. For example, in our car ad validator we can inject services. Go to **Application > Features > CarAds > Commands > Create** and add a `CreateCarAdCommandValidator` class:


```csharp
public class CreateCarAdCommandValidator : AbstractValidator<CreateCarAdCommand>
{
    public CreateCarAdCommandValidator(ICarAdRepository carAdRepository)
    {
        this.RuleFor(c => c.Category)
            .MustAsync(async (category, token) => await carAdRepository
                .GetCategory(category, token) != null)
            .WithMessage("'{PropertyName}' does not exist.");

        this.RuleFor(c => c.ImageUrl)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            .WithMessage("'{PropertyName}' must be a valid url.")
            .NotEmpty();

        this.RuleFor(c => c.TransmissionType)
            .Must(BeAValidTransmissionType)
            .WithMessage("'{PropertyName}' is not a valid transmission type.");
    }

    private static bool BeAValidTransmissionType(int transmissionType)
    {
        try
        {
            Enumeration.FromValue<TransmissionType>(transmissionType);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
```


## Add AutoMapper

With big output models mapping objects from one to another become complicated and troublesome to write. Using third-party tools like **AutoMapper** makes perfect sense because it will save us time. We will now integrate it into our architecture.

Install **AutoMapper.Extensions.Microsoft.DependencyInjection** to the **Application** project. Then create a `Mapping` folder in it. Create the `MappingProfile` class. This class is responsible to find all mappings in our solution by convention and register them so they will be executed automatically. And create `IMapFrom` interface again to the `Mapping` folder.

```csharp
public interface IMapFrom<T>
{
    void Mapping(Profile mapper) => mapper.CreateMap(typeof(T), this.GetType());
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly
            .GetExportedTypes()
            .Where(t => t
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            const string mappingMethodName = "Mapping";

            var methodInfo = type.GetMethod(mappingMethodName)
                             ?? type.GetInterface("IMapFrom`1")?.GetMethod(mappingMethodName);

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}
```

Go to **ApplicationConfiguration** and register **AutoMapper**

```csharp
public static IServiceCollection AddApplicationServices(
    this IServiceCollection services,
    IConfiguration? configuration)
    => services
        .Configure<ApplicationSettings>(
            configuration?.GetSection(nameof(ApplicationSettings))
                ?? throw new InvalidOperationException($"Missing {nameof(ApplicationSettings)} configuration"),
            options => options.BindNonPublicProperties = true)
        .AddAutoMapper(Assembly.GetExecutingAssembly())
        .AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        })
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
```

Our infrastructure code is ready. Let us now define a mapping. Go to `CarAdListingModel` and remove the constructor. **AutoMapper** works with private setters, so we are now going to encapsulate the class with them:

```csharp
public class CarAdListingModel : IMapFrom<CarAd>
{
    public Guid Id { get; private set; } = default;

    public string Manufacturer { get; private set; } = string.Empty;

    public string Model { get; private set; } = string.Empty;

    public string ImageUrl { get; private set; } = string.Empty;

    public string Category { get; private set; } = string.Empty;

    public decimal PricePerDay { get; private set; } = default;

    public void Mapping(Profile mapper)
        => mapper
            .CreateMap<CarAd, CarAdListingModel>()
            .ForMember(
                destination => destination.Manufacturer,
                cfg
                    => cfg.MapFrom(source => source.Manufacturer.Name))
            .ForMember(
                destination => destination.Category,
                cfg
                    => cfg.MapFrom(source => source.Category.Name));
}
```

To define a mapping in our infrastructure, all you need to do is add the IMapFrom interface:

```csharp
public class CarAdListingModel : IMapFrom<CarAd>
```

The default interface **Mapping** method will configure the map conventionally for us. If we want a custom logic, we can implement it manually.

```csharp
public void Mapping(Profile mapper)
    => mapper
        .CreateMap<CarAd, CarAdListingModel>()
        .ForMember(
            destination => destination.Manufacturer,
            cfg => cfg.MapFrom(source => source.Manufacturer.Name))
        .ForMember(
            destination => destination.Category,
            cfg => cfg.MapFrom(source => source.Category.Name));
```

Finally, in our CarAdRepository, we need to inject IMapper and project our query with it:

```csharp
public async Task<IEnumerable<CarAdListingModel>> GetCarAdListings(
    string? manufacturer = default,
    CancellationToken cancellationToken = default)
{
    var query = this.AllAvailable();
    
    if(!string.IsNullOrWhiteSpace(manufacturer))
    {
        query = query
            .Where(car => EF.Functions.Like(car.Manufacturer.Name, $"%{manufacturer}%"));
    }
    
    return await this.mapper
        .ProjectTo<CarAdListingModel>(query)
        .ToListAsync(cancellationToken);
}
```

An important rule to follow when using **AutoMapper** is to never map from input objects to domain entities. We should always use the domain methods and logic to do that transformation. It is perfectly fine to do the opposite – from domain objects to output models.


## Query Enhancements (Add [Specification Pattern](https://github.com/pirocorp/Object-Oriented-Design/tree/main/07.%20Specification))

Create `Specification` file in **Domain > Specifications**

```csharp
public abstract class Specification<T>
{
    private static readonly ConcurrentDictionary<string, Func<T, bool>> DelegateCache = new();

    private readonly List<string> cacheKey;

    protected Specification()
    {
        this.cacheKey = new List<string> { this.GetType().Name };
    }

    protected virtual bool Include => true;

    public virtual bool IsSatisfiedBy(T value)
    {
        if (!this.Include)
        {
            return true;
        }

        var func = DelegateCache.GetOrAdd(
            string.Join(string.Empty, this.cacheKey), 
            _ => this.ToExpression().Compile());

        return func(value);
    }

    public Specification<T> And(Specification<T> specification)
    {
        if (!specification.Include)
        {
            return this;
        }

        this.cacheKey.Add($"{nameof(this.And)}{specification.GetType()}");

        return new BinarySpecification(this, specification, true);
    }

    public Specification<T> Or(Specification<T> specification)
    {
        if (!specification.Include)
        {
            return this;
        }

        this.cacheKey.Add($"{nameof(this.Or)}{specification.GetType()}");

        return new BinarySpecification(this, specification, false);
    }

    public static implicit operator Expression<Func<T, bool>>(Specification<T> specification) 
        => specification.Include 
            ? specification.ToExpression()
            : value => true;

    public abstract Expression<Func<T, bool>> ToExpression();

    private class BinarySpecification : Specification<T>
    {
        private readonly Specification<T> left;
        private readonly Specification<T> right;
        private readonly bool andOperator;

        public BinarySpecification(Specification<T> left, Specification<T> right, bool andOperator)
        {
            this.right = right;
            this.left = left;
            this.andOperator = andOperator;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = this.left;
            Expression<Func<T, bool>> rightExpression = this.right;

            Expression body = this.andOperator 
                ? Expression.AndAlso(leftExpression.Body, rightExpression.Body) 
                : Expression.OrElse(leftExpression.Body, rightExpression.Body);

            var parameter = Expression.Parameter(typeof(T));
            body = (BinaryExpression)new ParameterReplacer(parameter).Visit(body);

            body = body ?? throw new InvalidOperationException("Binary expression cannot be null.");

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }

    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression parameter;

        internal ParameterReplacer(ParameterExpression parameter)
        {
            this.parameter = parameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
            => base.VisitParameter(this.parameter);
    }
}
```

Now create the following files:

![image](https://user-images.githubusercontent.com/34960418/225593492-b31cbcff-b7b4-4610-987a-6bf91ba2e815.png)

We have added three specifications – `CarAdByCategorySpecification`, `CarAdByManufacturerSpecification`, and `CarAdByPricePerDaySpecification`.

```csharp
public class CarAdByManufacturerSpecification : Specification<CarAd>
{
    private readonly string? manufacturer;

    public CarAdByManufacturerSpecification(string? manufacturer)
    {
        this.manufacturer = manufacturer;
    }

    protected override bool Include => this.manufacturer != null;

    public override Expression<Func<CarAd, bool>> ToExpression()
        => carAd => carAd.Manufacturer.Name.ToLower().Contains(this.manufacturer!.ToLower());
}
```

Basically, we need to inherit the base **Specification** abstract class, and then override `ToExpression`, in which we must provide the query. If we need to use the specification in a domain class – we can do it though the `IsSatisfiedBy` method. Additionally, if we want to specify for which situations the query should not execute (null values, for example), we can override the `Include` property too.

Go to the `CarAdRepository`, and change the `GetCarAdListings` method:

```csharp
public async Task<IEnumerable<CarAdListingModel>> GetCarAdListings(
    Specification<CarAd> specification,
    CancellationToken cancellationToken = default)
    => await this.AllAvailable()
        .Where(specification)
        .ProjectTo<CarAdListingModel>(this.mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
```

As a final step, go to the `SearchCarAdsQuery` to provide the specifications:

```csharp
public class SearchCarAdsQuery : IRequest<SearchCarAdsOutputModel>
{
    public string? Manufacturer { get; set; }

    public int? Category { get; set; }

    public decimal? MinPricePerDay { get; set; }

    public decimal? MaxPricePerDay { get; set; }

    public class SearchCarAdsQueryHandler : IRequestHandler<SearchCarAdsQuery, SearchCarAdsOutputModel>
    {
        private readonly ICarAdRepository carAdRepository;

        public SearchCarAdsQueryHandler(ICarAdRepository carAdRepository)
        {
            this.carAdRepository = carAdRepository;
        }

        public async Task<SearchCarAdsOutputModel> Handle(
            SearchCarAdsQuery request, 
            CancellationToken cancellationToken)
        {
            var carAdSpecification = new CarAdByManufacturerSpecification(request.Manufacturer)
                .And(new CarAdByCategorySpecification(request.Category))
                .And(new CarAdByPricePerDaySpecification(request.MinPricePerDay, request.MaxPricePerDay));

            var carAdListings = await this.carAdRepository.GetCarAdListings(
                carAdSpecification,
                cancellationToken);

            var totalCarAds = await this.carAdRepository.Total(cancellationToken);

            return new SearchCarAdsOutputModel(carAdListings, totalCarAds);
        }
    }
}
```

Keep in mind that there is no need to change every query to specification classes. For example, these queries would be an overkill for the specification pattern:

```csharp
public async Task<Category?> GetCategory(int categoryId, CancellationToken cancellationToken = default)
    => await this.Data.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

public async Task<Manufacturer?> GetManufacturer(
    string manufacturer,
    CancellationToken cancellationToken = default)
    => await this.Data.Manufacturers
        .FirstOrDefaultAsync(m => m.Name == manufacturer, cancellationToken);
```


## TODOs:

- **GET /Dealers/{id}** – Returns the dealer ID, name, phone number, and the total number of car ads. Public route.
- **PUT /Dealers/{id}** – A dealer can edit her name and phone number. Private route.
- **GET /CarAds** – Should have paging and sorting by price or by manufacturer. Should have filtering by dealer’s name. Should not return availability. Public route.
- **GET /CarAds/Mine** – Returns only the car ads created by the currently authenticated dealer. Includes availability too. It should allow the same filtering and sorting options like the **GET /CarAds** action. Private route.
- **GET /CarAds/Categories** – Returns all categories. The data should include their ID, name, description, and the total number of car ads in each category. Public route.
- **GET /CarAds/{id}** – Returns everything except availability about the car ad, including its dealer’s ID, name, and phone number. Public route.
- **PUT /CarAds/{id}** – A dealer can edit her car ads. Everything except availability should be editable. Private route.
- **PUT /CarAds/{id}/ChangeAvailability** – This route allows a dealer to change the availability state of her car ads. Private route.
- **DELETE /CarAds/{id}** – This route allows a dealer to delete one of her car ads. Private route.
- **POST /Identity/Login** – Returns the user’s dealer ID beside the token.
- **PUT /Identity/ChangePassword** – This route allows the currently authenticated user to change her password. Private route.
- **Implement the missing validation to the already implemented commands**.
- **Add unit and integration tests as you see fit**.

Hints:
- You may throw a `NotFoundException` in your queries in the cases where the provided ID is not found in the database.
- Editing domain entities should be done through exposed methods. Do not use setters as they should remain private.

Bonus Requirements:
- Add unit tests to the **Application** project. Assert configurations, commands, and queries.
- Add integration tests to the **Infrastructure** project. Assert all configurations and services.
- Add **CORS** to the system.
- Make **Scrutor** automatically register all services and initializers conventionally. For example, the `IIdentity` interface should be mapped to `IdentityService` and the `ICurrentUser` – to `CurrentUserService`.
- Add **MediatR** behaviors for request logging and performance tracking.
- Introduce database indexes. The **IsAvailable** column in the **CarAds** table is a perfect candidate. You filter by it on every search. Additionally, add **unique** indexes on the **category**, and **manufacturer** names.
- Introduce an internal `IRawQueries` interface in the **Persistence** infrastructure. Use **Dapper** by its implementation and write one of the commands with a raw SQL query.
- Add roles to the **Identity** system. Seed an administrator user to the database and give him the ability to change every piece of data in the application.
- Add a **base auditable entity**, which should store creating and editing information – user and date. Introduce soft delete and store who and when deleted the entity. Use the **SaveChanges** method to update the audit data.
- **Separate the repositories** – domain repositories and query repositories. The domain ones should live in the **Domain** project. The query ones should perform only queries mapped to an output model and should be placed in the **Application** project.
- **Introduce a repository with a memory cache**, which should serve as a proxy to the original one. Cache the car ad searching without any query parameters.
- Introduce a **GET /Statistics** action to return the total number of dealers and cars in the system. Introduce a response cache to the **Web** layer to improve the route’s performance.
- **Extract two bounded contexts** – **Identity** and **Dealers**. Separate the **DbContext** by using two interfaces, and do not overlap the logic in all layers above the database. Remove the **Dealer** navigational property but keep the **User-Dealer** relationship in the database. It should be broken only if microservices are going to be extracted.
- **Extract a bounded context** for the **statistics** and add a counter for car ad views, and car ad searches. Use domain events to update the data on every created car and registered dealer.


## Technologies:
- [AutoMapper](https://automapper.org/) - AutoMapper is a simple little library built to solve a deceptively complex problem - getting rid of code that mapped one object to another.
- [FluentValidations](https://docs.fluentvalidation.net/) - FluentValidation is a .NET library for building strongly-typed validation rules.
- [MediatR](https://github.com/jbogard/MediatR) - Simple mediator implementation in .NET. In-process messaging with no dependencies. Supports request/response, commands, queries, notifications and events, synchronous and async with intelligent dispatching via C# generic variance.
- [Scrutor](https://github.com/khellang/Scrutor) - Assembly scanning and decoration extensions for **Microsoft.Extensions.DependencyInjection**.
- [Swagger](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-7.0) - Swagger (OpenAPI) is a language-agnostic specification for describing REST APIs. It allows both computers and humans to understand the capabilities of a REST API without direct access to the source code.
- [Bogus](https://github.com/bchavez/Bogus) - Bogus is a simple fake data generator for .NET languages like C#, F# and VB.NET.
- [FakeItEasy](https://fakeiteasy.github.io/) - A .Net dynamic fake framework for creating all types of fake objects, mocks, stubs etc.
- [FluentAssertions](https://fluentassertions.com/) - A very extensive set of extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit tests.
- [xUnit](https://xunit.net/) - xUnit.net is a free, open source, community-focused unit testing tool for the .NET

