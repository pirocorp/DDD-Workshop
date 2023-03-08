namespace CarRentalSystem.Web.Services;

using System;
using System.Security.Claims;

using CarRentalSystem.Application.Contracts;

using Microsoft.AspNetCore.Http;

public class CurrentUserService : ICurrentUser
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.UserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? throw new InvalidOperationException("This request does not have an authenticated user.");
    }

    public string UserId { get; }
}
