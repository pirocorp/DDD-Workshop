namespace CarRentalSystem.Web.Common;

using System.Threading.Tasks;

using CarRentalSystem.Application.Common;

using Microsoft.AspNetCore.Mvc;

public static class ResultExtensions
{
    public static async Task<ActionResult<TData>> ToActionResult<TData>(this Task<TData> resultTask)
    {
        var result = await resultTask;

        if (result == null)
        {
            return new NotFoundResult();
        }

        return result;
    }

    public static async Task<ActionResult> ToActionResult(this Task<Result> resultTask)
    {
        var result = await resultTask;

        if (!result.Succeeded)
        {
            // TODO: Think is it better to return common Error Object fro like ValidationError and differentiate between validation errors and other errors (like credential errors).
            return new BadRequestObjectResult(result.Errors);
        }
            
        return new OkResult();
    }

    public static async Task<ActionResult<TData>> ToActionResult<TData>(this Task<Result<TData>> resultTask)
    {
        var result = await resultTask;

        if (!result.Succeeded)
        {
            // TODO: Think is it better to return common Error Object fro like ValidationError and differentiate between validation errors and other errors (like credential errors).
            return new BadRequestObjectResult(result.Errors);
        }

        return result.Data;
    }
}
