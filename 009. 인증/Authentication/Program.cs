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
            ValidateIssuerSigningKey = true,    // ������ ����
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:Key"]!)), // ������ Ű
            ValidateIssuer = false,             // Issuer ���� ����
            ValidateAudience = false            // Audience ���� ����
        };

        options.Events = new()                  // ���������� �ڵ����� ó���� ��ū�� ����
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("jwt-token"))       // �������� ���� ��ū�� �����ϸ�
                    context.Token = context.Request.Cookies["jwt-token"];   // ������ Ȯ���ؾ� �ϴ� ��ū ����
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

    return $"ȯ���մϴ�. {user.Username}��";
}).RequireAuthorization();


app.Run();