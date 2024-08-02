using System;
using System.Threading.Tasks;

namespace Common;

using Generics;
using Generics.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Security.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // Default to 500 if it's an unexpected exception
        var problemDetails = new ProblemDetails { Status = (int)code, Instance = context.Request.Path };
        LogException(exception);

        // Handle specific exception types
        switch (exception)
        {
            case DbEntityNotFoundException dbEntityNotFoundException:
                code = HttpStatusCode.NotFound;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "Entity Not Found";
                problemDetails.Detail = dbEntityNotFoundException.Message;
                break;
            case DbEntityEmptyListException dbEntityEmptyListException:
                code = HttpStatusCode.NotFound;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "Entity Empty List";
                problemDetails.Detail = dbEntityEmptyListException.Message;
                break;
            case UnauthorizedAccessException unauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "Unauthorized access exception";
                problemDetails.Detail = unauthorizedAccessException.Message;
                break;
            case AccessRightsViolationException accessRightsViolationException:
                code = HttpStatusCode.Forbidden;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "Access rights violation exception";
                problemDetails.Detail = accessRightsViolationException.Message;
                break;
            case AuthenticationException authenticationException:
                code = HttpStatusCode.BadRequest;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "Authentication exception";
                problemDetails.Detail = authenticationException.Message;
                break;
            case ArgumentException argumentException:
                code = HttpStatusCode.BadRequest;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "Argument exception";
                problemDetails.Detail = argumentException.Message;
                break;
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "Bad Request";
                problemDetails.Detail = validationException.Message;
                break;
            case DbEntityException dbEntityException:
                code = HttpStatusCode.InternalServerError;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "DB Entity Exception";
                problemDetails.Detail = dbEntityException.Message;
                break;
            case InternalServerErrorException internalServerErrorException:
                code = HttpStatusCode.InternalServerError;
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "Internal server error exception";
                problemDetails.Detail = internalServerErrorException.Message;
                break;
            default:
                problemDetails.Type = code.ToProblemDetailsType();
                problemDetails.Title = "An unexpected error occurred.";
                problemDetails.Detail = exception.Message; // Consider customizing or localizing the message.
                break;
        }

        problemDetails.Status = (int)code; // Assign the HTTP status code

        // Convert the ProblemDetails object to JSON
        var result = JsonConvert.SerializeObject(problemDetails);
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)code;

        // Write the JSON response
        return context.Response.WriteAsync(result);
    }

    private void LogException(Exception exception)
    {
        // Log the top-level exception
        logger.LogError("Exception: {Message}", exception.Message);

        // Recursively log inner exceptions
        var innerException = exception.InnerException;
        while (innerException != null)
        {
            logger.LogError("Inner Exception: {Message}", innerException.Message);
            innerException = innerException.InnerException;
        }
    }
}