var builder = WebApplication.CreateBuilder();

builder.Services.AddHostedService<BackgroundPost>();

var app = builder.Build();

app.MapPost("/post", (Info info) =>
{
    Console.WriteLine(info.Name);
    Console.WriteLine(info.Age);
});

app.Run();

public class BackgroundPost : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        HttpClient client = new();

        while (stoppingToken.IsCancellationRequested == false)
        {
            string[] ord = Console.ReadLine()!.Split();

            Info info = new()
            {
                Name = ord[0],
                Age = int.Parse(ord[1])
            };

            var res = await client.PostAsJsonAsync("http://localhost:8080/post", info);
        }
    }
}

public struct Info
{
    public string Name { get; set; }
    public int Age { get; set; }
}