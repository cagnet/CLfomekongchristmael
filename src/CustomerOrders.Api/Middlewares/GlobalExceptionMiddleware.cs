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
                case BusinessRuleException ex:
                    await HandleBusinessRuleException(ex, context); 
                    break;
                default:
                    throw;
            }
            
        }
    }


    private Task HandleBusinessRuleException(BusinessRuleException ex, HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return context.Response.WriteAsJsonAsync(new Problem(ex.Code, ex.Message));
    }
    
}