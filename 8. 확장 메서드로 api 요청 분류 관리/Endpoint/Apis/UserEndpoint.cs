using AutoMapper;
using FluentValidation;

using Endpoint.Models;
using Endpoint.DbContexts;
using Endpoint.Dtos;
using Microsoft.EntityFrameworkCore;


public static class UserEndpoint
{

    public static void RegisterUserApi(this WebApplication app)
    {
        var group = app.MapGroup("/user");

        group.MapGet("/{id}", async (UserDbContext db, IMapper mapper, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

            UserDto userDto = mapper.Map<UserDto>(user);

            return Results.Ok(userDto);
        });

        group.MapGet("/list", async (UserDbContext db, IMapper mapper) =>
        {
            List<UserDto> userDtos = mapper.Map<List<UserDto>>(await db.Users.ToListAsync());

            return Results.Ok(userDtos);
        });

        group.MapPost("", async (UserDbContext db, IMapper mapper, IValidator<CreateUserDto> validator, CreateUserDto createUser) =>
        {
            var results = validator.Validate(createUser);

            if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

            User user = mapper.Map<User>(createUser);

            db.Users.Add(user);

            await db.SaveChangesAsync();

            UserDto userDto = mapper.Map<UserDto>(user);

            return Results.Created($"/user/{user.Id}", userDto);
        });

        group.MapPatch("/{id}", async (UserDbContext db, IMapper mapper, IValidator<UpdateUserDto> validator, UpdateUserDto updateUser, int id) =>
        {
            var results = validator.Validate(updateUser);

            if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

            mapper.Map(updateUser, user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (UserDbContext db, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}