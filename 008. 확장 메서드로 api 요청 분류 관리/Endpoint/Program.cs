using Microsoft.EntityFrameworkCore;
using FluentValidation;

using Endpoint.DbContexts;
using Endpoint.Validators;
using Endpoint.Dtos;
using Endpoint.Models;


var builder = WebApplication.CreateBuilder();

string? connectionString = new ConfigurationBuilder().AddUserSecrets<Program>().Build()["ConnectionStrings:Default"];
connectionString ??= builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();

builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<UserDbContext, UserDto>();
    config.CreateMap<CreateUserDto, User>();
    config.CreateMap<UpdateUserDto, User>()
    .ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null && srcMember.ToString() != ""));
});

var app = builder.Build();

app.RegisterUserApi();

app.Run();