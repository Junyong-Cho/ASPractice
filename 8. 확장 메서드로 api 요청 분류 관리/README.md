# Endpoint �з� ����

���ݱ��� �츮�� ������ api ��û�� ó���ϴ� CRUD �޼������ ��û�� ���������� ó���ϴ� �޼���� Ư�� url�� ���� ��û�� ó���ϴµ� �� url�� ��������Ʈ(Endpoint)��� �Ѵ�.

�׸��� ASP.NET Core�� api�� �����ϴ� ����� ��Ʈ�ѷ� ��İ� �̴ϸ� api ������� ũ�� 2������ �ִµ� �츮�� ����ϰ� �ִ� ����� �ֻ��� ������ �����ϴ� �̴ϸ� api ����̴�.

�̴ϸ� api�� �������δ� ������ api ������ ������ ó���ؾ� �ϴ� ��������Ʈ�� ������ ���� �ϳ��� �ֻ��� ������ ��� ��������Ʈ�� ó���ϴ� api�� �����ϰ� �Ǹ� �ڵ��� ������ ��������� ������ �ִ�.

![1. User C R U D](../dummy/9%20api%20�з�%20����/1.%20user%20CRUD.png)
<small><small>(������ ������ /user�� �����ϴ� CRUD ��û�鸸 �ص� ����� ���� ���� �����Ѵ�.)</small></small>  

���� ������ ���ϵ��� ����Ǵ� �κ��� �ִ� �� �׸��� ���� ��������Ʈ����(/user/id, /user/list, /user) �ϳ��� ��� �����Ͽ� ��Ʈ�ѷ� ��� apió�� api ������ ���ϰ� �� �� �ֵ��� �����غ��ڴ�.

## Ȯ�� �޼���

Ȯ�� �޼���� Ư���� Ŭ������ �߰��ϴ� �޼��带 ���Ѵ�.  
���� ��� int Ŭ������ print()��� �޼��带 �߰��غ��ڴ�.

```C#
1.Print();                      // 1 ���

Console.WriteLine(1.Plus(3));   // 1+3�� ����� 4 ���

1.Plus(3).Print();              // ���� �����ϰ� ����

static class Extension
{
    static public void Print(this int a)
    {
        Console.WriteLine(a);
    }

    static public int Plus (this int a, int b)
    {
        return a + b;
    }
}
```

�� �ڵ�� int Ŭ������ Print() Plus() �޼��带 �߰��ϵ��� ������ Ȯ�� �޼����̴�.  

Print() �޼���� ���� �ν��Ͻ� ������ ����ϴ� �޼����̰� Plus() �޼���� �Ű������� ���� ���� �ν��Ͻ� ���ΰ� ���ؼ� return�ϴ� �޼����̴�.

���� C#�� int Ŭ������ �������� ���� �޼���������� Ȯ�� �޼��带 ���������ν� .Print() .Plus() �޼��带 �߰����� ���̴�.

��ó�� �ڵ带 �ۼ��ϰ� �����ϸ� �ּ��� ǥ���� ���� ���� ��µ� ���̴�.

���⼭ ```this int a```��� ����� �Ű������� �ٷ� int Ŭ������ �߰��Ѵٴ� ���̴�. ```this string st```�� string Ŭ������ �߰��ϰ� ���������� ```this WebApplication app```�̸� WebApplication Ŭ������ �߰��ϰڴٴ� ���� �ȴ�.

�߿��� ���� Ŭ������ �޼��� ��� static���� �����ؾ� �ϴ� ���̴�.

�츮�� app.MapGet�� ���� api�� ������ �� �� app�� �ٷ� WebApplication Ŭ������ �ν��Ͻ��� WebApplication Ŭ������ Ȯ�� �޼��带 �����ϸ� Api���� �и��ؼ� ������ �� �ִ�.

## Ȯ�� �޼��带 �̿��� api �з�

���� ������Ʈ�� Apis��� ���͸��� �����ϰ� ������ UserEndpoint.cs ������ ������ �ش�.

![2. ���͸� ����](../dummy/9%20api%20�з�%20����/2.%20���͸�%20����.png)

�׸��� UserEndpoint.cs ���Ͽ� ������ ���� �ʾ��� �ۼ��Ѵ�.

```C#
public static class UserEndpoint
{
    public static RegisterUserApi(this WebApplication app)
    {

    }
}
```

�� ���� Program.cs�� �����Ǿ��� app.MapGet app.MapPost �� /user�� ���۵Ǵ� ��������Ʈ�� ó���ϴ� �޼������ ���� �߶󳽴�.

�׸��� �߶� �ڵ���� RegisterUserApi �޼��忡 �ٿ��ִ´�.

```C#
public static class UserEndpoint
{

    public static void RegisterUserApi(this WebApplication app)
    {   
        app.MapGet("/user/{id}", async (UserDbContext db, IMapper mapper, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("�ش��ϴ� id�� ������ �����ϴ�.");

            UserDto userDto = mapper.Map<UserDto>(user);

            return Results.Ok(userDto);
        });

        app.MapGet("/user/list", async (UserDbContext db, IMapper mapper) =>
        {
            List<UserDto> userDtos = mapper.Map<List<UserDto>>(await db.Users.ToListAsync());

            return Results.Ok(userDtos);
        });

        app.MapPost("/user", async (UserDbContext db, IMapper mapper, IValidator<CreateUserDto> validator, CreateUserDto createUser) =>
        {
            var results = validator.Validate(createUser);

            if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

            User user = mapper.Map<User>(createUser);

            db.Users.Add(user);

            await db.SaveChangesAsync();

            UserDto userDto = mapper.Map<UserDto>(user);

            return Results.Created($"/user/{user.Id}", userDto);
        });

        app.MapPatch("/user/{id}", async (UserDbContext db, IMapper mapper, IValidator<UpdateUserDto> validator, UpdateUserDto updateUser, int id) =>
        {
            var results = validator.Validate(updateUser);

            if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("�ش��ϴ� id�� ������ �����ϴ�.");

            mapper.Map(updateUser, user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/user/{id}", async (UserDbContext db, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("�ش��ϴ� id�� ������ �����ϴ�.");

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
```

�̷��� ������ �� �۵������� ���� �� �����ϱ� ���ϰ� �ڵ带 ������ �� ���̴�.

MapGroup�� �̿��ؼ� /user url�� ��� �����غ��ڴ�.

�켱 app ������ Ű���� Ŀ���� �ΰ� ���־� ��Ʃ����� ctrl+r+r ����Ű�� �̿��Ͽ� app ������ ���� group���� �ٲ��ش�.

![3. ���� ��](../dummy/9%20api%20�з�%20����/3.%20����%20��.png)
<small><small>���� ��</small></small>

![4. ���� ��](../dummy/9%20api%20�з�%20����/4.%20����%20��.png)
<small><small>���� ��</small></small>

�� ���� ```this WebApplication group```�� ������ app���� �ٲ��ش�. 

![5. This Webapp App](../dummy/9%20api%20�з�%20����/5.%20this%20webapp%20app.png)

�� ���� ```var group = app.MapGroup("/user");``` �ڵ带 �� ù �ٿ� �Է��Ѵ�.

![6. Map Group](../dummy/9%20api%20�з�%20����/6.%20MapGroup.png)

�׸��� ��� ��������Ʈ url���� /user�� �����Ѵ�.

�׷��� �ϸ� ���������� ������ ���� �ڵ尡 �ϼ��ȴ�.  
��������Ʈ ������ ������ �������� ���̴�.

```C#
public static class UserEndpoint
{

    public static void RegisterUserApi(this WebApplication app)
    {
        var group = app.MapGroup("/user");

        group.MapGet("/{id}", async (UserDbContext db, IMapper mapper, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("�ش��ϴ� id�� ������ �����ϴ�.");

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

            if (user == null) return Results.NotFound("�ش��ϴ� id�� ������ �����ϴ�.");

            mapper.Map(updateUser, user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (UserDbContext db, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("�ش��ϴ� id�� ������ �����ϴ�.");

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
```

## Program.cs�� ���

���������� ������ api���� �����ϴ� �޼��尡 ������ RegisterUserApi() �޼��带 �ֻ��� ���� �߰����ֱ⸸ �ϸ� �ȴ�.

```C#
var app = builder.Build();

app.RegisterUserApi();

app.Run();
```

���������� �ֻ��������� ������ ���� ���� �ڵ常 ����ϰ� ���� �� ���̴�.

```C#
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
```

�̷��� �ϸ� �̴ϸ� api�� ������ api �����̶�� ������ �����ϸ鼭 �ֻ������� ���������� ������ �ذ��� �� �ִ�.

# ������

api ��û�� ó���ϴ� �޼������ ��������Ʈ���� �з��غ��Ҵ�.