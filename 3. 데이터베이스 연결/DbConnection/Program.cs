using DbConnection.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>().Build();

string? connectionString = config["ConnectionStrings:Default"];
connectionString ??= builder.Configuration.GetConnectionString("Default");
// connectionString이 null이면 ??= 오른쪽 값 대입

//Console.WriteLine(connectionString);
//int t = 1;
//if (t == 1) return;
// 위 코드로 잘 불러왔는지 확인해보기

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

app.Run();