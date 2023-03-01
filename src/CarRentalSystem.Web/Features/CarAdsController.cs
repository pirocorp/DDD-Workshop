namespace CarRentalSystem.Web.Features;

using System.Threading.Tasks;

using CarRentalSystem.Application.Features.CarAds.Queries.Search;

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
    public async Task<ActionResult<SearchCarAdsOutputModel>>Get([FromQuery] SearchCarAdsQuery query) 
        => await this.Send(query);
}
