using DbApi.DbContexts;
using Microsoft.EntityFrameworkCore;

using DbApi.Models;

var builder = WebApplication.CreateBuilder();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>().Build();

string? connectionString = config["ConnectionStrings:Default"];
connectionString ??= builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// 비동기 프로그래밍 테스트 api_____________________________________________________________________________________

app.MapGet("/", () => "index page");

app.MapGet("/sync", () =>
{
    Thread.Sleep(5000);         // 현재 메서드가 쓰레드를 차지한 채로 5초 대기

    return "sync";
});

app.MapGet("/async", async () =>
{

    await Task.Delay(5000);     // 현재 메서드를 백그라운드에 잠시 보내고 5초 후 불러옴

    return "async";
});

// Get(Read)______________________________________________________________________________________________________

app.MapGet("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);      // db에서 id에 맞는 User 데이터를 찾아올 동안 다른 작업 처리 가능하도록 함

    if (user == null) return Results.NotFound();

    return Results.Ok(user);
});

app.MapGet("/user/all", async (UserDbContext db) =>
{
    List<User> users = await db.users.ToListAsync();    // db에서 User 리스트를 가져오는 동안 다른 작업 처리 가능하도록 함

    return Results.Ok(users);
});

// Post(Create)_____________________________________________________________________________________________________

app.MapPost("/user/{id}", async (UserDbContext db, User user, int id) =>
{
    if (db.users.Any(u => u.Id == id)) return Results.Conflict();
                                        // id가 존재하면 409(충돌) 반환

    db.users.Add(user);

    await db.SaveChangesAsync();

    return Results.Created($"/user/{id}", user);
});

// Patch, Put(Update)_______________________________________________________________________________________________

app.MapPut("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();        // id가 존재하지 않으면 404 반환

    db.Entry(user).CurrentValues.SetValues(updateUser); // 전체 데이터 수정

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPatch("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();        // id가 존재하지 않으면 404 반환

    if (updateUser.Username != null) user.Username = updateUser.Username;   // 수정해야 할 데이터가 있으면 수정 없으면 무시
    if (updateUser.Email != null) user.Email = updateUser.Email;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Delete(Delete)___________________________________________________________________________________________________

app.MapDelete("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();    // id가 존재하지 않으면 404 반환

    db.users.Remove(user);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();