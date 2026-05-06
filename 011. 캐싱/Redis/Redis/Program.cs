using StackExchange.Redis;

var builder = WebApplication.CreateBuilder();

builder.Services.AddSingleton(sp =>
{
    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
});

var app = builder.Build();

app.MapGet("/set/{set}", async (string set, ConnectionMultiplexer con) =>
{
    string[] st = set.Split("-");

    string key = st[0];
    string value = st[1];
    int sec = int.Parse(st[2]);

    var redis = con.GetDatabase();

    TimeSpan exp = TimeSpan.FromSeconds(sec);

    await redis.StringSetAsync(key, value, exp);

    return Results.Ok("Ok");
});

app.MapGet("/get/{key}", async (string key, ConnectionMultiplexer con) =>
{
    var redis = con.GetDatabase();

    var value = await redis.StringGetAsync(key);

    if (value.IsNull == true)
        return Results.Ok("NULL");

    return Results.Ok(value.ToString());
});

app.Run();