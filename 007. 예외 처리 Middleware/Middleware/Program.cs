using Middleware.ExceptionHandler;
using Middleware.Middlewares;

var builder = WebApplication.CreateBuilder();

// 1번 방법
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// 2번 방법
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapGet("/error", () => 
{
    throw new Exception("Error! error!");
});

app.Run();