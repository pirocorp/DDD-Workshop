namespace CarRentalSystem.Web.Features;

using System.Collections.Generic;
using System.Linq;

using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Domain.Models.CarAds;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[ProducesResponseType(StatusCodes.Status200OK)]
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
