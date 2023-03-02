namespace CarRentalSystem.Fakes.WebApplicationFactories;

using System.Linq;

using CarRentalSystem.Fakes.Infrastructure.Identity;
using CarRentalSystem.Infrastructure.Identity;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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
