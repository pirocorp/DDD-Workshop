namespace CarRentalSystem.Web;

using System;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
    private IMediator? mediator;

    protected IMediator Mediator
        => this.mediator 
            ??= this.HttpContext.RequestServices.GetService<IMediator>() 
            ?? throw new InvalidOperationException("IMediator service is not registered in the DI container.");
}
