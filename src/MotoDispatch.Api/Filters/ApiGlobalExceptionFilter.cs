using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Domain.Exception;

namespace MotoDispatch.Api.Filters;

public class ApiGlobalExceptionFilter(IHostEnvironment env)
    : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();
        var exception = context.Exception;

        if (env.IsDevelopment())
            details.Extensions.Add("StackTrace", exception.StackTrace);

        if (exception is EntityValidationException)
        {
            details.Title = "One or more validation errors ocurred";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnprocessableEntity";
            details.Detail = exception!.Message;
        }
        else if (exception is NotFoundException)
        {
            details.Title = "Not Found";
            details.Status = StatusCodes.Status404NotFound;
            details.Type = "NotFound";
            details.Detail = exception!.Message;
        }
        else if (exception is DuplicateEntityException)
        {
            details.Title = "Duplicate Entity";
            details.Status = StatusCodes.Status400BadRequest;
            details.Type = "DuplicateEntity";
            details.Detail = exception!.Message;
        }
        else if (exception is RelatedAggregateException)
        {
            details.Title = "Invalid Related Aggregate";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "RelatedAggregate";
            details.Detail = exception!.Message;
        }
        else
        {
            details.Title = "An unexpected error ocurred";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnexpectedError";
            details.Detail = exception.Message;
        }

        context.HttpContext.Response.StatusCode = (int) details.Status;
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }
}