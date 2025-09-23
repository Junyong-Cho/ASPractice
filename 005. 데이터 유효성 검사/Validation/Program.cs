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
    var result = await validator.ValidateAsync(user);                           // ��ȿ�� �˻�
    
    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());                // ��ȿ���� ������ ������ �޼����� �Բ� ��ȯ

    if (await db.users.AnyAsync(u => u.Id == id)) return Results.Conflict();    // id ���� Ȯ��

    /*
    .....
    ..... db ���� �ڵ� �ۼ�
    */
    return Results.Created($"/user/{id}", user);                                // ���� �۵�
});

app.Run();