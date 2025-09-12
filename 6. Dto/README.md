# DTO ������ AutoMapper

�츮�� ���ݱ��� CRUD�� �ְ� �޾Ҵ� �����ʹ� db�� ����Ǿ� �ִ� ������ ��ü�� �ְ�޾Ҵ�.  
�׷��� ���� �����Ϳ� �н����� �� ����Ǹ� �� �Ǵ� �����͵� ������ �� �ִ�.  
�׷��� �Ǹ� User ��ü�� �״�� ������ ��� ���Ȼ��� ������ ���� ���̴�.  
���� DTO(Data Transfer Object : ������ ���޿� ��ü)�� ���� ������ �� �����͸� �������� �Ͽ� �����͸� �����԰� ���ÿ� �ΰ��� �����͸� �����ϰ� ��ȣ�� �� �ִ�.  

## UserDto

�׷� User �����͸� �����ϱ� ���� UserDto.cs ������ �����ڴ�.  
������Ʈ�� Dtos ���͸��� �����ϰ� ���ο� UserDto.cs ������ �����.  
�츮�� ������ User ��ü�� �Ӽ�(Id, Username, Password, Email) �� ������ �����ͷ� Username�� Email�� �����غ��Ҵ�.

![1. ���͸� ����](../dummy/7%20DTO/1.%20���͸�%20����.png)

```C#
namespace Dto.Dtos;

public class UserDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
```


## CreateUserDto

Dto�� �����͸� ���� ���� ���Ǵ� ���� �ƴϴ�.  
�츮�� �׵��� �ǽ��ϸ鼭 ����ߴ� User ��ü�� Id �Ӽ� ���� ���� �⺻Ű�μ� db���� �ڵ����� �����Ǵ� ���ε� �� ���� ������ �� �Է¹��� �ʿ䰡 ����.  
�̷��� ������ �� �ʿ��� �����͵鸸 �޵��� �ϴ� ���� ������ dto�� CreateUserDto�� ����� �� ���̴�.

Dtos ���͸��� CreateUserDto.cs ������ �����Ѵ�.

![2. Createuserdto �߰�](../dummy/7%20DTO/2.%20createuserdto%20�߰�.png)

CreaterUserDto�� ���̵� ������ �����͸� �Է¹��� ���̴�. ������ ���� �ۼ��Ѵ�.

```C#
namespace Dto.Dtos;

public class CreateUserDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
}
```

## UpdateUserDto

�����Ϳ� ���� ������ ��û�� ������ Dto�� �̿��Ͽ� �� �� �ִ�.  
CreateUserDto�� �ٸ��� UpdateUserDto�� null���� ����ִ� �Ӽ��� ������ ���� �ʵ��� �� ���̹Ƿ� CreateUserDto�� ���� ���� ������ ���� ������ �־�� �Ѵ�.

```C#
namespace Dto.Dtos;

public class UpdateUserDto
{
    string? Username { get; set; }
    string? Password { get; set; }
    string? Email { get; set; }
}
```


## AutoMapper

���� ������ CreateUserDto�� User�� User�� UserDto�� ��ȯ���Ѽ� �����͸� �ְ�޾ƾ� �Ѵ�.  
���� �����ڸ� ���� ��ȯ�ǵ��� ������ ���� ������ �ڵ��� �������� ���� ���̺귯���� ��ġ�غ��ڴ�.

nuget ����Ʈ�� AutoMapper�� �˻��Ѵ�.

![101. Auto Mapper](../dummy/7%20DTO/101.%20AutoMapper.png)

���̺귯���� ��ġ�� �ְ� �� ������ ���� .cs ������ ������ �ʰ� ���� �ܰ迡�� �������ְڴ�.

```C#
var builder = WebApplication.CreateBuilder();

// ...
// �ٸ� ���� �ڵ�

builder.Services.AddAutoMapper(confg =>
{
    confg.CreateMap<User, UserDto>();           // User�� UserDto�� ����
    confg.CreateMap<CreateUserDto, User>();     // CreateUserDto�� User�� ����
    confg.CreateMap<UpdateUserDto, User>()      // UpdateUserDto�� User�� ����
    .ForAllMembers(option => option.Condition((src, dest, srcMember) => srcMember != null));
});

var app = builder.Build();
```

## Validator ����

���� �ð��� User ��ü�� Validator�� �����ߴµ� Dto�� �����ϸ鼭 Post Ȥ�� Update�� �޴� User�� ���� ��ȿ���� �˻��� �� �ʿ䰡 ��������.  
���� Dto���� ��ȿ���� �˻����ִ� Validator�� �ʿ��ϴ�.

Validators ���͸� ������ UserValidator.cs�� �����ϰ� UpdateUserValidator.cs�� CreateUserValidator.cs�� �������ش�.

![3. ���͸� ���� ](../dummy/7%20DTO/3.%20���͸�%20����%20.png)

```C#
using Dto.Dtos;
using FluentValidation;

namespace Dto.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Username)                                        // null ����
            .NotNull().WithMessage("�ʼ� �Է�");

        RuleFor(u => u.Password)                                        // null ����
            .NotNull().WithMessage("�ʼ� �Է�")
            .Length(8, 30).WithMessage("8���� �̻�")                    // ���� �� ����
            .Matches(@"[a-z]").WithMessage("�ҹ��� ����")               // �ҹ���, �빮��, ����, Ư������ �˻�
            .Matches(@"[A-Z]").WithMessage("�빮�� ����")
            .Matches(@"[0-9]").WithMessage("���� ����")
            .Matches(@"[!@#$.]").WithMessage("Ư���� Ư������ ����");

        RuleFor(u => u.Email).EmailAddress().WithMessage("�ùٸ��� ���� �̸��� ����"); // null�̸� �ǳʶ�
    }
}
```


CreateUserValidator�� �ݵ�� �ʿ��� �Ӽ��� NotNull�� ���� �˻��ϵ��� �Ͽ��� Email �Ӽ��� null���� �� �����ϵ��� �Ͽ���.


```C#
using FluentValidation;

using Dto.Dtos;

namespace Dto.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.Username)                                        // null�� �ƴ� ���� ����ִ��� �˻�
            .NotEmpty().WithMessage("��� ������ �ȵ�")
            .When(u => u.Username != null);

        RuleFor(u => u.Password)                                        // null�̸� �ǳʶ�
            .Length(8, 30).WithMessage("8���� �̻�")
            .Matches(@"[A-Z]").WithMessage("�빮�� ����")
            .Matches(@"[a-z]").WithMessage("�ҹ��� ����")
            .Matches(@"[0-9]").WithMessage("���� ����")
            .Matches(@"[!@#$.,]").WithMessage("������ Ư������ ����");

        RuleFor(u => u.Email)                                           // null�̸� �ǳʶ�
            .EmailAddress().WithMessage("�ùٸ��� ���� �̸��� ����");
    }
}
```

UpdateUserValidator�� null�� �ƴ� �� �� ������ ���� �˻��ϵ��� ���־���.

## api ����

���� api���� �����͸� Dto�� �ְ�޵��� �����غ��ڴ�.

## Get ��û
```C#
app.MapGet("/user/{id}", async (UserDbContext db, int id, IMapper mapper) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    UserDto userDto = mapper.Map<UserDto>(user);

    return Results.Ok(userDto);
});
```

db���� User�� ã�� �ڿ� UserDto�� ���ν��Ѽ� return�ϵ��� �����ߴ�.

## Post��û

```C#
app.MapPost("/user", async (UserDbContext db, IMapper mapper, IValidator<CreateUserDto> validator, CreateUserDto updateUser) =>
{
    var results = validator.Validate(updateUser);

    if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

    User user = mapper.Map<User>(updateUser);

    db.users.Add(user);

    await db.SaveChangesAsync();

    UserDto userDto = mapper.Map<UserDto>(user);

    return Results.Created($"/user/{user.Id}", userDto);
});
```

������ ������ �´� Validator�� ������ �ڿ� ��ȿ�� �˻� �� User ��ü�� �����Ͽ� db�� �����ϵ��� ������ ���Ҵ�.

## Put ��û
```C#
app.MapPut("/user/{id}", async (UserDbContext db, IMapper mapper, UpdateUserDto updateUser, IValidator<UpdateUserDto> validator, int id) =>
{
    var results = validator.Validate(updateUser);

    if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    User pUser = mapper.Map<User>(updateUser);

    pUser.Id = user.Id;

    db.Entry(user).CurrentValues.SetValues(pUser);

    await db.SaveChangesAsync();

    return Results.NoContent();
});
```

���������� ��ȿ�� �˻� �� ��ü�� �����Ͽ� �����ϵ��� �����ߴ�.


## Patch ��û
```C#
app.MapPatch("/user/{id}", async (UserDbContext db, IMapper mapper, IValidator<UpdateUserDto> validator, UpdateUserDto updateUser, int id) =>
{
    var results = validator.Validate(updateUser);

    if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    mapper.Map(updateUser, user);
    await db.SaveChangesAsync();

    return Results.NoContent();
});
```
Patch ��û�� Put ��û�� ���̰� �ֵ��� �����ߴ�. Put ��û�� Email���� null ������ �ԷµǴ� Email �Ӽ������� ������� �Ͽ��� Patch�� null ���� �������� �ʵ��� AutoMapper�� �����Ͽ����Ƿ� Map() �޼���� �����ߴ�.

## Delete ��û

```C#
app.MapDelete("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    db.users.Remove(user);
    await db.SaveChangesAsync();

    return Results.NoContent();
});
```

Delete ��û�� �ܼ��� id�� �޾Ƽ� �����ϵ��� �����ߴ�.

## Request ������Ʈ

�� api�� ����� �����ϴ��� Request.cs ������ ������ ���� �ۼ��ߴ�.

```C#
using System.Net.Http.Json;
using static System.Console;

HttpClient client = new();

while (true)
{
    HttpResponseMessage response = null;
    Write(">> ");
    switch (ReadLine())
    {
        case "1":
            CreateDto cUser = new(input(), input(), input());

            response = await client.PostAsJsonAsync("http://localhost:5009/user",cUser);

            break;
        case "2":
            {
                int id = int.Parse(ReadLine());

                UpdateDto uUser = new(input(), input(), input());

                response = await client.PutAsJsonAsync($"http://localhost:5009/user/{id}", uUser);
            }
            break;
        case "3":
            {
                int id = int.Parse(ReadLine());
                
                UpdateDto uUser = new(input(), input(), input());

                response = await client.PatchAsJsonAsync($"http://localhost:5009/user/{id}", uUser);
            }
            break;
        case "4":
            {
                int id = int.Parse(ReadLine());

                response = await client.DeleteAsync($"http://localhost:5009/user/{id}");
            }
            break;
        default:
            return;
    }

    WriteLine($"\n{response.StatusCode}\n");

    if (!response.IsSuccessStatusCode)
    {
        WriteLine($"\n{await response.Content.ReadAsStringAsync()}\n");
    }
}



string? input()     // null ���� �Է¹ޱ� ���� ������ �޼���
{
    string? st = ReadLine();
    if (st == "null") return null;
    return st;
}

record CreateDto(string? Username, string? Password, string? Email);

record UpdateDto(string? Username, string? Password, string? Email);
```

## Id Ȯ���ϴ� ���

CreateUserDto�� ������ �� id ���� �����ϰ� ���������� Get ��û�� ���� ���� id�� �ʿ��ϴ�.  
������ �������� id�� Ȯ���ϱ� ���ؼ��� db�� ���� �����Ͽ� �˾Ƴ� �� �ִ�.

![5. ����](../dummy/7%20DTO/5.%20����.png)

Request.cs ������ �����Ͽ� �����͸� �����Ѵ�.

�͹̳ο� ```psql -U postgres``` ��ɾ�� DBMS�� ������ ������  
```\c db�̸�``` ��ɾ�� db�� �����ϰ�  
```select * from users;``` ��ɾ �����ϸ� �ڵ����� ������ Id�� Ȯ���� �� �ִ�.

![6. Id Ȯ��](../dummy/7%20DTO/6.%20id%20Ȯ��.png)

ã�Ƴ� ���̵�� Get ��û�� �غ� �� �ִ�.

![7. G Et ��û](../dummy/7%20DTO/7.%20GEt%20��û.png)

# ������

Dto�� ����� Validator�� ���� �����Ҵ�.