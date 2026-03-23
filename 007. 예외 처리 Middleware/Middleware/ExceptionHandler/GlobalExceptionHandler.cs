using Microsoft.AspNetCore.Diagnostics;

namespace Middleware.ExceptionHandler;

public class GlobalExceptionHandler : IExceptionHandler
{
    //GlobalExceptionHandlerMiddleware의 HandleException 메서드 부분
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Fatal Server Error : {exception.Message}");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            Status = 500,
            Message = "Internal Server Error",
            Detail = exception.Message
        });

        return true;
    }
}
