namespace CarRentalSystem.Web.Features;

using System.Threading.Tasks;

using CarRentalSystem.Application.Contracts;
using CarRentalSystem.Application.Features.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentity identity;

    public IdentityController(IIdentity identity)
    {
        this.identity = identity;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="model">User input model</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Identity/Register
    ///     {
    ///         "email": "example@example.com",
    ///         "password": "password"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">User is registered successfully.</response>
    /// <response code="400">User is not registered.</response>
    [HttpPost]
    [Route(nameof(Register))]
    public async Task<ActionResult> Register(UserInputModel model)
    {
        var result = await this.identity.Register(model);

        if (!result.Succeeded)
        {
            return this.BadRequest(result.Errors);
        }

        return this.Ok();
    }

    /// <summary>
    /// Logs a user in the system
    /// </summary>
    /// <param name="model">User input model</param>
    /// <returns>JSON Web Token</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Identity/Login
    ///     {
    ///         "email": "example@example.com",
    ///         "password": "password"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns JSON Web Token.</response>
    /// <response code="400">Invalid credentials.</response>
    [HttpPost]
    [Route(nameof(Login))]
    public async Task<ActionResult<LoginOutputModel>> Login(UserInputModel model)
    {
        var result = await this.identity.Login(model);

        if (!result.Succeeded)
        {
            return this.BadRequest(result.Errors);
        }

        return result.Data;
    }

    /// <summary>
    /// Get current user
    /// </summary>
    /// <returns>Email of the current user</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /Identity
    /// 
    /// </remarks>
    /// <response code="200">Email of the current user.</response>
    /// <response code="401">User is not authenticated.</response>
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return this.Ok(this.User.Identity?.Name);
    }
}
