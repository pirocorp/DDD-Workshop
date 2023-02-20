# DDD Workshop

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














