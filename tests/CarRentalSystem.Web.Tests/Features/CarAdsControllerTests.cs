namespace CarRentalSystem.Web.Tests.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using CarRentalSystem.Application.Features.CarAds.Queries.Search;
using CarRentalSystem.Fakes.Domain.Models.CarAds;
using CarRentalSystem.Fakes.WebApplicationFactories;
using CarRentalSystem.Infrastructure.Persistence;
using CarRentalSystem.Startup;

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

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
    public async Task CarAdsControllerSearchReturnsEmptyArrayWhenNoAdsAreFound()
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
    public async Task CarAdsControllerSearchReturnsCollectionOfCarAds()
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
    public async Task CarAdsControllerSearchReturnsCorrectAdsByManufacturer()
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
