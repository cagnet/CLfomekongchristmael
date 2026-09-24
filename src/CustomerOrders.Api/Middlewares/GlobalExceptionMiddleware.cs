using CustomerOrders.Business.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CustomerOrders.Api.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next)
{

    public async Task InvokeAsync(HttpContext context, ILogger<GlobalExceptionMiddleware> logger)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case BusinessConflictException ex:
                    await HandleBusinessConflictException(ex, context); 
                    break;
                case BusinessRuleException ex:
                    await HandleBusinessRuleException(ex, context); 
                    break;
                default:
                    await HandleUnknowException(e, context, logger);
                    break;
            }
            
        }
    }


    private Task HandleBusinessRuleException(BusinessRuleException ex, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return context.Response.WriteAsJsonAsync(new Problem(ex.Code, ex.Message));
    }

    private Task HandleBusinessConflictException(BusinessRuleException ex, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status409Conflict;
        return context.Response.WriteAsJsonAsync(new Problem(ex.Code, ex.Message));
    }
    
    private Task HandleUnknowException(Exception ex, HttpContext context, ILogger<GlobalExceptionMiddleware> logger)
    {
        logger.LogError(ex, "@ An unexpected error occurs");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return context.Response.WriteAsJsonAsync(new Problem("internal_server_error", "Internal server error. Retry later !"));

    }
    
}