namespace CarRentalSystem.Web.Features;

using System.Linq;

using CarRentalSystem.Application;
using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Domain.Models.CarAds;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

public class CarAdsController : ApiController
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
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /CarAds
    /// 
    /// </remarks>
    /// <response code="200">Returns all available car ads.</response>
    [HttpGet]
    public object Get() => new
    {
        Settings = this.settings,
        CarAds = this.carAds
            .All()
            .Where(c => c.IsAvailable),
    };
}
