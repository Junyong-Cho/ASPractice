using Middleware.Middlewares;

var builder = WebApplication.CreateBuilder();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapGet("/error", () => 
{
    throw new Exception("Error! error!");
});

app.Run();