namespace CarRentalSystem.Infrastructure.Persistence;

using System.Reflection;

using CarRentalSystem.Domain.Models.CarAds;
using CarRentalSystem.Domain.Models.Dealers;
using CarRentalSystem.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

internal class CarRentalDbContext : IdentityDbContext<User>
{
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
        : base(options)
    { }

    public DbSet<CarAd> CarAds => this.Set<CarAd>();

    public DbSet<Category> Categories => this.Set<Category>();

    public DbSet<Manufacturer> Manufacturers => this.Set<Manufacturer>();

    public DbSet<Dealer> Dealers => this.Set<Dealer>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
