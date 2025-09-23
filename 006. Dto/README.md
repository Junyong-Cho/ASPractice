# DTO 생성과 AutoMapper

우리가 지금까지 CRUD로 주고 받았던 데이터는 db에 저장되어 있는 데이터 자체를 주고받았다.  
그러나 유저 데이터에 패스워드 등 유출되면 안 되는 데이터도 존재할 수 있다.  
그렇게 되면 User 객체를 그대로 전송할 경우 보안상의 문제가 생길 것이다.  
따라서 DTO(Data Transfer Object : 데이터 전달용 객체)를 만들어서 보내야 할 데이터만 보내도록 하여 데이터를 절약함과 동시에 민감한 데이터를 안전하게 보호할 수 있다.  

## UserDto

그럼 User 데이터를 전달하기 위한 UserDto.cs 파일을 만들어보겠다.  
프로젝트에 Dtos 디렉터리를 생성하고 내부에 UserDto.cs 파일을 만든다.  
우리가 구현한 User 객체의 속성(Id, Username, Password, Email) 중 전송할 데이터로 Username과 Email로 설정해보았다.

<img width="213" height="212" alt="1  디렉터리 구조" src="https://github.com/user-attachments/assets/84afa894-314a-4f57-bad6-9761c4b843ea" />

```C#
namespace Dto.Dtos;

public class UserDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
```


## CreateUserDto

Dto는 데이터를 보낼 때만 사용되는 것이 아니다.  
우리가 그동안 실습하면서 사용했던 User 객체의 Id 속성 같은 경우는 기본키로서 db에서 자동으로 생성되는 값인데 이 값을 생성될 때 입력받을 필요가 없다.  
이렇게 생성될 때 필요한 데이터들만 받도록 하는 유저 생성용 dto인 CreateUserDto도 만들어 볼 것이다.

Dtos 디렉터리에 CreateUserDto.cs 파일을 생성한다.

<img width="276" height="265" alt="2  createuserdto 추가" src="https://github.com/user-attachments/assets/594f4405-707e-4256-917f-4719c5c55461" />

CreaterUserDto는 아이디를 제외한 데이터를 입력받을 것이다. 다음과 같이 작성한다.

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

데이터에 대한 수정을 요청할 때에도 Dto를 이용하여 할 수 있다.  
CreateUserDto와 다르게 UpdateUserDto는 null값이 들어있는 속성은 수정을 하지 않도록 할 것이므로 CreateUserDto와 생긴 것은 같지만 따로 생성해 주어야 한다.

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

이제 생성한 CreateUserDto를 User로 User를 UserDto로 변환시켜서 데이터를 주고받아야 한다.  
직접 생성자를 통해 변환되도록 구현할 수도 있으나 코드의 간결함을 위해 라이브러리를 설치해보겠다.

nuget 사이트에 AutoMapper를 검색한다.

<img width="1164" height="421" alt="4  AutoMapper" src="https://github.com/user-attachments/assets/aa9bd22f-2b69-4a01-be65-5801f88ce261" />

라이브러리를 설치해 주고 이 매핑은 따로 .cs 파일을 만들지 않고 빌드 단계에서 매핑해주겠다.

```C#
var builder = WebApplication.CreateBuilder();

// ...
// 다른 설정 코드

builder.Services.AddAutoMapper(confg =>
{
    confg.CreateMap<User, UserDto>();           // User를 UserDto에 매핑
    confg.CreateMap<CreateUserDto, User>();     // CreateUserDto를 User에 매핑
    confg.CreateMap<UpdateUserDto, User>()      // UpdateUserDto를 User에 매핑
    .ForAllMembers(option => option.Condition((src, dest, srcMember) => srcMember != null));
});

var app = builder.Build();
```

UpdateUserDto는 속성이 null이 아닌 경우에만 User에 매핑하도록 구현하였다.

## Validator 구현

이전 시간에 User 객체에 Validator를 구현했는데 Dto를 구현하면서 Post 혹은 Update로 받는 User에 대한 유효성을 검사해 줄 필요가 없어졌다.  
따라서 Dto별로 유효성을 검사해주는 Validator가 필요하다.

Validators 디렉터리 밑으로 UserValidator.cs는 삭제하고 UpdateUserValidator.cs와 CreateUserValidator.cs를 생성해준다.

<img width="281" height="286" alt="3  디렉터리 구조 " src="https://github.com/user-attachments/assets/ee59d046-2f6d-4f60-9fa2-651258b86468" />

```C#
using Dto.Dtos;
using FluentValidation;

namespace Dto.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Username)                                        // null 불허
            .NotNull().WithMessage("필수 입력");

        RuleFor(u => u.Password)                                        // null 불허
            .NotNull().WithMessage("필수 입력")
            .Length(8, 30).WithMessage("8글자 이상")                    // 글자 수 제한
            .Matches(@"[a-z]").WithMessage("소문자 포함")               // 소문자, 대문자, 숫자, 특수문자 검사
            .Matches(@"[A-Z]").WithMessage("대문자 포함")
            .Matches(@"[0-9]").WithMessage("숫자 포함")
            .Matches(@"[!@#$.]").WithMessage("특정한 특수문자 포함");

        RuleFor(u => u.Email).EmailAddress().WithMessage("올바르지 않은 이메일 형식"); // null이면 건너뜀
    }
}
```


CreateUserValidator는 반드시 필요한 속성은 NotNull을 통해 검사하도록 하였고 Email 속성은 null값일 때 무시하도록 하였다.


```C#
using FluentValidation;

using Dto.Dtos;

namespace Dto.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.Username)                                        // null이 아닐 때만 비어있는지 검사
            .NotEmpty().WithMessage("비어 있으면 안됨")
            .When(u => u.Username != null);

        RuleFor(u => u.Password)                                        // null이면 건너뜀
            .Length(8, 30).WithMessage("8글자 이상")
            .Matches(@"[A-Z]").WithMessage("대문자 포함")
            .Matches(@"[a-z]").WithMessage("소문자 포함")
            .Matches(@"[0-9]").WithMessage("숫자 포함")
            .Matches(@"[!@#$.,]").WithMessage("지정된 특수문자 포함");

        RuleFor(u => u.Email)                                           // null이면 건너뜀
            .EmailAddress().WithMessage("올바르지 않은 이메일 형식");
    }
}
```

UpdateUserValidator는 null이 아닌 값 즉 수정할 값만 검사하도록 해주었다.

## api 구현

이제 api에서 데이터를 Dto로 주고받도록 구현해보겠다.

## Get 요청
```C#
app.MapGet("/user/{id}", async (UserDbContext db, int id, IMapper mapper) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();

    UserDto userDto = mapper.Map<UserDto>(user);

    return Results.Ok(userDto);
});
```

db에서 User를 찾은 뒤에 UserDto로 매핑시켜서 return하도록 구현했다.

## Post요청

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

데이터 유형에 맞는 Validator를 설정한 뒤에 유효성 검사 후 User 객체에 매핑하여 db에 저장하도록 구현해 보았다.

## Put 요청
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

마찬가지로 유효성 검사 후 전체를 복사하여 저장하도록 구현했다.


## Patch 요청
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
Patch 요청은 Put 요청과 차이가 있도록 구현했다. Put 요청은 Email같이 null 값으로 입력되는 Email 속성까지도 복사토록 하였고 Patch는 null 값은 매핑하지 않도록 AutoMapper에 구현하였으므로 Map() 메서드로 구현했다.

## Delete 요청

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

Delete 요청은 단순히 id만 받아서 삭제하도록 구현했다.

## Request 프로젝트

각 api가 제대로 동작하는지 Request.cs 파일을 다음과 같이 작성했다.

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



string? input()     // null 값을 입력받기 위해 정의한 메서드
{
    string? st = ReadLine();
    if (st == "null") return null;
    return st;
}

record CreateDto(string? Username, string? Password, string? Email);

record UpdateDto(string? Username, string? Password, string? Email);
```

## Id 확인하는 방법

CreateUserDto를 전송할 때 id 값을 제외하고 전송하지만 Get 요청을 보낼 때는 id가 필요하다.  
생성한 데이터의 id를 확인하기 위해서는 db에 직접 접속하여 알아낼 수 있다.

<img width="262" height="157" alt="5  생성" src="https://github.com/user-attachments/assets/759f2768-a5b7-4aa4-813c-fedabe164b87" />

Request.cs 파일을 실행하여 데이터를 저장한다.

터미널에 ```psql -U postgres``` 명령어로 DBMS에 접속한 다음에  
```\c db이름``` 명령어로 db에 접속하고  
```select * from users;``` 명령어를 실행하면 자동으로 생성된 Id를 확인할 수 있다.

<img width="475" height="267" alt="6  id 확인" src="https://github.com/user-attachments/assets/01b6da32-6b55-4d06-9050-40902191eac9" />

찾아낸 아이디로 Get 요청을 해볼 수 있다.

<img width="328" height="190" alt="7  GEt 요청" src="https://github.com/user-attachments/assets/98d58bfd-0e28-4951-9aa7-0c6025bb593f" />

# 마무리

Dto를 만들고 Validator를 새로 만들어보았다.
