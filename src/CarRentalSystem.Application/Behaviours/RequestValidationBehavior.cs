﻿namespace CarRentalSystem.Application.Behaviours;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CarRentalSystem.Application.Exceptions;

using FluentValidation;
using MediatR;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var tasks = this
            .validators
            .Select(v => v.ValidateAsync(request, cancellationToken));

        var errors = (await Task.WhenAll(tasks))
            .SelectMany(v => v.Errors)
            .Where(f => f != null)
            .ToList();

        if (errors.Count != 0)
        {
            throw new ModelValidationException(errors);
        }

        return await next();
    }
}
