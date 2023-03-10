namespace CarRentalSystem.Web.Features;

using System.Threading.Tasks;

using CarRentalSystem.Application.Features.Identity.Commands.CreateUser;
using CarRentalSystem.Application.Features.Identity.Commands.LoginUser;

using Microsoft.AspNetCore.Mvc;

public class IdentityController : ApiController
{
    /// <summary>
    /// Register new user
    /// </summary>
    /// <param name="command">Create User Command</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Identity/Register
    ///     {
    ///         "email": "example@example.com",
    ///         "password": "password",
    ///         "name": "Piroman Piromanov",
    ///         "phoneNumber": "+359 888888888"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">User is registered successfully.</response>
    /// <response code="400">User is not registered.</response>
    [HttpPost]
    [Route(nameof(Register))]
    public async Task<ActionResult> Register(CreateUserCommand command)
        => await this.Send(command);

    /// <summary>
    /// Logs a user in the system
    /// </summary>
    /// <param name="command">Create User Command</param>
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
    public async Task<ActionResult<LoginOutputModel>> Login(LoginUserCommand command)
        => await this.Send(command);
}
