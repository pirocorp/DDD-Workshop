namespace CarRentalSystem.Web.Features;

using System.Threading.Tasks;

using CarRentalSystem.Application.Features.CarAds.Commands.Create;
using CarRentalSystem.Application.Features.CarAds.Queries.Search;
using CarRentalSystem.Web.Middleware.ValidationExceptionHandler;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

public class CarAdsController : ApiController
{
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /CarAds?manufacturer=AUDI
    /// 
    /// </remarks>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Returns Available Car Ads", 
        Description = "Filter available car ads by manufacturer, category and price per day.")]
    [SwaggerResponse(
        StatusCodes.Status200OK, 
        "Returns available car ads", 
        typeof(SearchCarAdsOutputModel))]
    public async Task<ActionResult<SearchCarAdsOutputModel>>Search([FromQuery] SearchCarAdsQuery query) 
        => await this.Send(query);

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
    [HttpPost]
    [Authorize]
    [SwaggerOperation(
        "Creates a Car Ad", 
        Description = "Creates a car ad when provided with valid data.")]
    [SwaggerResponse(
        StatusCodes.Status200OK, 
        "Returns created car ad's id", 
        typeof(CreateCarAdOutputModel))]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Returns list of errors if car ad cannot be created.",
        typeof(ValidationErrors))]
    [SwaggerResponse(
        StatusCodes.Status401Unauthorized,
        "Request does not have an authenticated user.")]
    public async Task<ActionResult<CreateCarAdOutputModel>> Create(CreateCarAdCommand command)
        => await this.Send(command);
}
