namespace CarRentalSystem.Fakes.WebApplicationFactories;

using System.Data.Common;
using System.Linq;

using CarRentalSystem.Infrastructure.Persistence;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            var carRentalDbInitializerDescriptor = services.SingleOrDefault(
                d => d.ImplementationType == typeof(CarRentalDbInitializer));

            services.Remove(carRentalDbInitializerDescriptor!);

            services.AddDbContext<CarRentalDbContext>((container, options) =>
            {
                options.UseInMemoryDatabase(this.databaseId);
                //options.UseSqlServer("Server=PIRO\\SQLEXPRESS2019;Database=DDD-Workshop-IntegrationTests;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True");
            });
        });

        builder.UseEnvironment("Development");
    }
}
