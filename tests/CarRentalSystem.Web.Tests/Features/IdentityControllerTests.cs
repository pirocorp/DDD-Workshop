namespace CarRentalSystem.Web.Tests.Features;

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using CarRentalSystem.Fakes.Infrastructure.Identity;
using CarRentalSystem.Fakes.WebApplicationFactories;
using CarRentalSystem.Infrastructure.Persistence;
using CarRentalSystem.Startup;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

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

    [Fact]
    public async Task RegisterShouldCreateUser()
    {
        // Arrange
        this.webFactory = new CustomWebApplicationFactory<Program>(Guid.NewGuid().ToString());
        this.httpClient = this.webFactory.CreateDefaultClient();

        var payload = JsonConvert.SerializeObject(new
        {
            Email = "valid@example.com",
            Password = "valid123",
            Name = "Piroman",
            PhoneNumber = "+359 888888888"
        });

        // Act
        var response = await this.PostToEndpoint("/Identity/Register", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var scope = this.webFactory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
        (await dbContext.Users.AnyAsync()).Should().BeTrue();
    }

    private async Task<HttpResponseMessage> PostToEndpoint(string endpoint, string payload)
        => await this.httpClient.PostAsync(
            endpoint,
            new StringContent(payload, Encoding.UTF8, "application/json"));

    private record TokenResult(string Token);
}
