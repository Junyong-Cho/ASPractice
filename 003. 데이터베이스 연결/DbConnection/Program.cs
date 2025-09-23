using DbConnection.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>().Build();

string? connectionString = config["ConnectionStrings:Default"];
connectionString ??= builder.Configuration.GetConnectionString("Default");
// connectionString�� null�̸� ??= ������ �� ����

//Console.WriteLine(connectionString);
//int t = 1;
//if (t == 1) return;
// �� �ڵ�� �� �ҷ��Դ��� Ȯ���غ���

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

app.Run();