using AutoMapper;
using FluentValidation;

using Dto.Models;
using Dto.Dtos;
using Dto.Validators;
using Dto.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

string? connectionString = config["ConnectionStrings:Default"];

connectionString ??= builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(confg =>
{
    confg.CreateMap<User, UserDto>();           // User를 UserDto에 매핑
    confg.CreateMap<CreateUserDto, User>();     // CreateUserDto를 User에 매핑
    confg.CreateMap<UpdateUserDto, User>()      // UpdateUserDto를 User에 매핑
    .ForAllMembers(option => option.Condition((src, dest, srcMember) => srcMember != null));
});

builder.Services.AddValidatorsFromAssemblyContaining(typeof(UpdateUserValidator));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreateUserValidator));

var app = builder.Build();

app.MapGet("/user/{id}", async (UserDbContext db, int id, IMapper mapper) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    UserDto userDto = mapper.Map<UserDto>(user);

    return Results.Ok(userDto);
});

app.MapPost("/user", async (UserDbContext db, IMapper mapper, IValidator<CreateUserDto> validator, CreateUserDto updateUser) =>
{
    var results = validator.Validate(updateUser);

    if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

    User user = mapper.Map<User>(updateUser);

    db.users.Add(user);

    await db.SaveChangesAsync();

    UserDto userDto = mapper.Map<UserDto>(user);

    return Results.Created($"/user/{user.Id}", userDto);
});

app.MapPut("/user/{id}", async (UserDbContext db, IMapper mapper, UpdateUserDto updateUser, IValidator<UpdateUserDto> validator, int id) =>
{
    var results = validator.Validate(updateUser);

    if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    User pUser = mapper.Map<User>(updateUser);

    pUser.Id = user.Id;

    db.Entry(user).CurrentValues.SetValues(pUser);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPatch("/user/{id}", async (UserDbContext db, IMapper mapper, IValidator<UpdateUserDto> validator, UpdateUserDto updateUser, int id) =>
{
    var results = validator.Validate(updateUser);

    if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    mapper.Map(updateUser, user);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    db.users.Remove(user);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();