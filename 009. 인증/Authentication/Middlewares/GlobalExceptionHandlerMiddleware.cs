using System.Net;
using System.Text.Json;

namespace Authentication.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception e)
        {
            await HandleException(context, e);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "text/json";

        var error = new
        {
            context.Response.StatusCode,
            Message = "서버 오류",
            Detail = ex.Message
        };

        var detail = JsonSerializer.Serialize(error);

        return context.Response.WriteAsync(detail);
    }
}
