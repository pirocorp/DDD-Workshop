namespace CarRentalSystem.Web.Features;

using System.Threading.Tasks;

using CarRentalSystem.Application.Features.CarAds.Commands.Create;
using CarRentalSystem.Application.Features.CarAds.Queries.Search;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class CarAdsController : ApiController
{
    /// <summary>
    /// Returns All Available Car Ads
    /// </summary>
    /// <param name="query">Search Car Ads Query</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /CarAds?Manufacturer=manufacturer
    /// 
    /// </remarks>
    /// <response code="200">Returns all available car ads.</response>
    [HttpGet]
    public async Task<ActionResult<SearchCarAdsOutputModel>>Search([FromQuery] SearchCarAdsQuery query) 
        => await this.Send(query);

    /// <summary>
    /// Create Car Ad
    /// </summary>
    /// <param name="command">Car Ad</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Identity/Register
    ///     {
    ///           "manufacturer": "AUDI",
    ///           "model": "RS7",
    ///           "category": 3,
    ///           "imageUrl": "https://hips.hearstapps.com/hmg-prod/images/2024-audi-rs7-performance-motion-front-2-1669663936.jpg",
    ///           "pricePerDay": 250.00,
    ///           "climateControl": true,
    ///           "numberOfSeats": 5,
    ///           "transmissionType": 2
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns Created Car Ad's Id</response>
    /// <response code="400">Returns Errors If Car Ad Cannot be Created</response>
    /// <response code="401">Request does not have an authenticated user.</response>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreateCarAdOutputModel>> Create(CreateCarAdCommand command)
        => await this.Send(command);
}
