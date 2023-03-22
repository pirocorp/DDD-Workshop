namespace CarRentalSystem.Web.Middleware.ValidationExceptionHandler;

using System.Collections.Generic;

internal class ValidationErrors
{
    public ValidationErrors(
        bool validationDetails, 
        IDictionary<string, string[]> errors)
    {
        this.ValidationDetails = validationDetails;
        this.Errors = errors;
    }

    public bool ValidationDetails { get; }

    public IDictionary<string, string[]> Errors { get; }
}
