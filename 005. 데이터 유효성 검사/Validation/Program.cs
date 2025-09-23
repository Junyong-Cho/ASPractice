using Microsoft.EntityFrameworkCore;
using FluentValidation;

using Validation.Models;
using Validation.DbContexts;
using Validation.Validators;

var builder = WebApplication.CreateBuilder();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>().Build();

string? connectionString = config["ConnectionStrings:Default"];
connectionString ??= builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

var app = builder.Build();

app.MapPost("/user/{id}",async (UserDbContext db, IValidator<User> validator, User user, int id) =>
{
    var result = await validator.ValidateAsync(user);                           // 유효성 검사
    
    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());                // 유효성에 문제가 있으면 메세지와 함께 반환

    if (await db.users.AnyAsync(u => u.Id == id)) return Results.Conflict();    // id 존재 확인

    /*
    .....
    ..... db 저장 코드 작성
    */
    return Results.Created($"/user/{id}", user);                                // 정상 작동
});

app.Run();