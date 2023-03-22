namespace CarRentalSystem.Web.Features;

using System.Collections.Generic;
using System.Threading.Tasks;

using CarRentalSystem.Application.Features.Identity.Commands.CreateUser;
using CarRentalSystem.Application.Features.Identity.Commands.LoginUser;
using CarRentalSystem.Web.Middleware.ValidationExceptionHandler;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

public class IdentityController : ApiController
{
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
    [HttpPost]
    [Route(nameof(Register))]
    [SwaggerOperation(
        "Register a new user",
        "Register a new user when provided with valid data.")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "User is registered successfully.")]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Returns list of errors if user cannot be registered.",
        typeof(ValidationErrors))]
    public async Task<ActionResult> Register(CreateUserCommand command)
        => await this.Send(command);


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
    [HttpPost]
    [Route(nameof(Login))]
    [SwaggerOperation(
        "Login a user in the system",
        "If email and passwords are correct logins a user in the system by returning JWT token")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "User is logged in successfully and JWT token is returned.",
        typeof(LoginOutputModel))]
    [SwaggerResponse(
        StatusCodes.Status400BadRequest,
        "Returns list of errors",
        typeof(List<string>))]
    public async Task<ActionResult<LoginOutputModel>> Login(LoginUserCommand command)
        => await this.Send(command);
}
