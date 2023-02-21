namespace CarRentalSystem.Web.Features;

using System.Collections.Generic;
using System.Linq;

using CarRentalSystem.Domain.Models.CarAds;
using CarRentalSystem.Domain.Models.Dealers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CarAdsController : ControllerBase
{
    private static readonly Dealer Dealer = new ("Dealer", "+359123456789");

    /// <summary>
    /// Returns Dummy Dealer
    /// </summary>
    [HttpGet]
    public IEnumerable<CarAd> Get() => Dealer
        .CarAds
        .Where(c => c.IsAvailable);
}
