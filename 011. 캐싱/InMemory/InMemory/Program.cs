using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ServerCache>();

var app = builder.Build();

app.MapGet("/set/{set}", (string set, ServerCache cache) =>
{
    string[] split = set.Split('-');

    string key = split[0];
    string value = split[1];
    int sec = int.Parse(split[2]);

    DateTimeOffset offset = DateTimeOffset.Now.AddSeconds(sec);

    cache.SetEx(key, value, offset);

    return Results.Ok("OK");
});

app.MapGet("/get/{key}", (string key, ServerCache cache) =>
{
    string value = cache.Get(key) ?? "Empty";

    return Results.Ok(value);
});

app.Run();

public class ServerCache
{
    readonly IMemoryCache _cache;

    public ServerCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetEx(string key, string value, DateTimeOffset offset)
    {
        _cache.Set(key, value, offset);
    }

    public string? Get(string key)
    {
        return _cache.Get<string>(key);
    }
}