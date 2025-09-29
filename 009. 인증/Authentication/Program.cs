using Authentication.Apis;
using Authentication.DbContexts;
using Authentication.Dtos.AuthDtos;
using Authentication.Middlewares;
using Authentication.Models;
using Authentication.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder();

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]!);
});

builder.Services.AddValidatorsFromAssemblyContaining<SignupUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SigninUserValidator>();

builder.Services.AddAutoMapper(options =>
{
    options.CreateMap<SignupUserDto, User>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,    // 인증서 검증
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:Key"]!)), // 인증서 키
            ValidateIssuer = false,             // Issuer 검증 안함
            ValidateAudience = false            // Audience 검증 안함
        };

        options.Events = new()                  // 웹브라우저가 자동으로 처리한 토큰을 지정
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("jwt-token"))       // 서버에서 보낸 토큰이 존재하면
                    context.Token = context.Request.Cookies["jwt-token"];   // 서버가 확인해야 하는 토큰 지정
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.AddAuthRequest();

app.MapGet("/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/index.html");
});

app.MapGet("/myname", async (HttpContext context, UserDbContext db) =>
{
    string? id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    User? user = await db.Users.FindAsync(int.Parse(id!));

    return $"환영합니다. {user.Username}님";
}).RequireAuthorization();


app.Run();