using Microsoft.Extensions.Options;
using Mongo.DbSettings;
using Mongo.Models;
using Mongo.Services;

var builder = WebApplication.CreateBuilder();

builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

MongoDbSettings mongoDbSetting = new();

builder.Configuration.GetSection("MongoDbSettings").Bind(mongoDbSetting);

builder.Services.AddSingleton(await MongoDbService.CreateAsync(Options.Create(mongoDbSetting)));

var app = builder.Build();

app.MapGet("/get", async (MongoDbService db) =>
{
    List<User> users = await db.GetAllAsync();
    return Results.Ok(users);
});

app.MapGet("/post", async (MongoDbService db) =>
{
    User user = new()
    {
        Username = "홍길동",
        UserId = "honggildong",
        Password = "패스워드",
        Email = "hong@example.com"
    };

    await db.PostAsync(user);

    return Results.Ok("post success");
});

app.MapGet("/put", async (MongoDbService db) =>
{
    User user = await db.GetAsync("honggildong");
    user.Username = "고길동";
    await db.PutAsync(user);

    return Results.Ok("put success");
});

app.MapGet("/del", async (MongoDbService db) =>
{
    User user = await db.GetAsync("honggildong");

    await db.DeleteAsync(user);

    return Results.Ok("delete success");
});

app.Run();