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

// �񵿱� ���α׷��� �׽�Ʈ api_____________________________________________________________________________________

app.MapGet("/", () => "index page");

app.MapGet("/sync", () =>
{
    Thread.Sleep(5000);         // ���� �޼��尡 �����带 ������ ä�� 5�� ���

    return "sync";
});

app.MapGet("/async", async () =>
{

    await Task.Delay(5000);     // ���� �޼��带 ��׶��忡 ��� ������ 5�� �� �ҷ���

    return "async";
});

// Get(Read)______________________________________________________________________________________________________

app.MapGet("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);      // db���� id�� �´� User �����͸� ã�ƿ� ���� �ٸ� �۾� ó�� �����ϵ��� ��

    if (user == null) return Results.NotFound();

    return Results.Ok(user);
});

app.MapGet("/user/all", async (UserDbContext db) =>
{
    List<User> users = await db.users.ToListAsync();    // db���� User ����Ʈ�� �������� ���� �ٸ� �۾� ó�� �����ϵ��� ��

    return Results.Ok(users);
});

// Post(Create)_____________________________________________________________________________________________________

app.MapPost("/user/{id}", async (UserDbContext db, User user, int id) =>
{
    if (db.users.Any(u => u.Id == id)) return Results.Conflict();
                                        // id�� �����ϸ� 409(�浹) ��ȯ

    db.users.Add(user);

    await db.SaveChangesAsync();

    return Results.Created($"/user/{id}", user);
});

// Patch, Put(Update)_______________________________________________________________________________________________

app.MapPut("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();        // id�� �������� ������ 404 ��ȯ

    db.Entry(user).CurrentValues.SetValues(updateUser); // ��ü ������ ����

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPatch("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();        // id�� �������� ������ 404 ��ȯ

    if (updateUser.Username != null) user.Username = updateUser.Username;   // �����ؾ� �� �����Ͱ� ������ ���� ������ ����
    if (updateUser.Email != null) user.Email = updateUser.Email;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Delete(Delete)___________________________________________________________________________________________________

app.MapDelete("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();    // id�� �������� ������ 404 ��ȯ

    db.users.Remove(user);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();