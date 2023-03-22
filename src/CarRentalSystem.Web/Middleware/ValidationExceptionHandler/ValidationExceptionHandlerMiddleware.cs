namespace CarRentalSystem.Web.Middleware.ValidationExceptionHandler;

using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using CarRentalSystem.Application.Exceptions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public class ValidationExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ValidationExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        var result = string.Empty;

        switch (exception)
        {
            case ModelValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = SerializeObject(new ValidationErrors(
                    true, 
                    validationException.Errors));
                break;
            case NotFoundException _:
                code = HttpStatusCode.NotFound;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (string.IsNullOrEmpty(result))
        {
            result = SerializeObject(new[] { exception.Message });
        }

        return context.Response.WriteAsync(result);
    }

    private static string SerializeObject(object obj)
        => JsonSerializer.Serialize(obj, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        });
}

public static class ValidationExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<ValidationExceptionHandlerMiddleware>();
}
