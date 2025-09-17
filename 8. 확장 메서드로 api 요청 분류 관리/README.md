# Endpoint 분류 관리

지금까지 우리가 구현한 api 요청을 처리하는 CRUD 메서드들은 요청을 최종적으로 처리하는 메서드로 특정 url로 들어온 요청을 처리하는데 이 url을 엔드포인트(Endpoint)라고 한다.

그리고 ASP.NET Core로 api를 구현하는 방법은 컨트롤러 방식과 미니멀 api 방식으로 크게 2가지가 있는데 우리가 사용하고 있는 방법은 최상위 문으로 구현하는 미니멀 api 방식이다.

미니멀 api의 장점으로는 간단한 api 구현이 있지만 처리해야 하는 엔드포인트가 많아질 수록 하나의 최상위 문에서 모든 엔드포인트를 처리하는 api를 구현하게 되면 코드의 관리가 어려워지는 문제가 있다.

<img width="767" height="824" alt="1  user CRUD" src="https://github.com/user-attachments/assets/befd7d6d-b357-4dba-ab4a-004079098dc4" />

(간단히 구현한 /user로 시작하는 CRUD 요청들만 해도 상당히 많은 양을 차지한다.)

따라서 관리가 편하도록 공통되는 부분이 있는 위 그림과 같은 엔드포인트들을(/user/id, /user/list, /user) 하나로 묶어서 관리하여 컨트롤러 기반 api처럼 api 관리를 편하게 할 수 있도록 구현해보겠다.

## 확장 메서드

확장 메서드란 특정한 클래스에 추가하는 메서드를 말한다.  
예를 들면 int 클래스에 print()라는 메서드를 추가해보겠다.

```C#
1.Print();                      // 1 출력

Console.WriteLine(1.Plus(3));   // 1+3의 결과인 4 출력

1.Plus(3).Print();              // 위와 동일하게 동작

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

위 코드는 int 클래스에 Print() Plus() 메서드를 추가하도록 구현된 확장 메서드이다.  

Print() 메서드는 정수 인스턴스 본인을 출력하는 메서드이고 Plus() 메서드는 매개변수로 받은 수를 인스턴스 본인과 더해서 return하는 메서드이다.

실제 C#의 int 클래스에 구현되지 않은 메서드들이지만 확장 메서드를 구현함으로써 .Print() .Plus() 메서드를 추가해준 것이다.

위처럼 코드를 작성하고 실행하면 주석에 표시해 놓은 값이 출력될 것이다.

여기서 ```this int a```라고 선언된 매개변수가 바로 int 클래스에 추가한다는 뜻이다. ```this string st```면 string 클래스에 추가하고 마찬가지로 ```this WebApplication app```이면 WebApplication 클래스에 추가하겠다는 뜻이 된다.

중요한 것은 클래스와 메서드 모두 static으로 선언해야 하는 것이다.

우리가 app.MapGet과 같이 api를 구현할 때 이 app이 바로 WebApplication 클래스의 인스턴스로 WebApplication 클래스의 확장 메서드를 구현하면 Api들을 분리해서 관리할 수 있다.

## 확장 메서드를 이용한 api 분류

먼저 프로젝트에 Apis라는 디렉터리를 생성하고 밑으로 UserEndpoint.cs 파일을 생성해 준다.

<img width="315" height="320" alt="2  디렉터리 구조" src="https://github.com/user-attachments/assets/7d548bba-1160-4e20-802d-2e3542413365" />

그리고 UserEndpoint.cs 파일에 다음과 같이 초안을 작성한다.

```C#
public static class UserEndpoint
{
    public static RegisterUserApi(this WebApplication app)
    {

    }
}
```

그 다음 Program.cs에 구현되었던 app.MapGet app.MapPost 등 /user로 시작되는 엔드포인트를 처리하는 메서드들을 전부 잘라낸다.

그리고 잘라낸 코드들을 RegisterUserApi 메서드에 붙여넣는다.

```C#
public static class UserEndpoint
{

    public static void RegisterUserApi(this WebApplication app)
    {   
        app.MapGet("/user/{id}", async (UserDbContext db, IMapper mapper, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

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

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

            mapper.Map(updateUser, user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/user/{id}", async (UserDbContext db, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
```

이렇게 끝내도 잘 작동하지만 조금 더 관리하기 편하게 코드를 수정해 줄 것이다.

MapGroup을 이용해서 /user url을 묶어서 관리해보겠다.

우선 app 변수에 키보드 커서를 두고 비주얼 스튜디오의 ctrl+r+r 단축키를 이용하여 app 변수를 전부 group으로 바꿔준다.

<img width="869" height="452" alt="3  변경 전" src="https://github.com/user-attachments/assets/7f351a8f-90aa-4c30-96fc-3880ce991f05" />  

변경 전

<img width="995" height="431" alt="4  변경 후" src="https://github.com/user-attachments/assets/88130185-c30e-4edf-9d99-0fbd35a7aa6b" />  

변경 후

그 다음 ```this WebApplication group```의 변수만 app으로 바꿔준다. 

<img width="1083" height="483" alt="5  this webapp app" src="https://github.com/user-attachments/assets/690904c8-84b5-4c06-baba-318d738bedbf" />

그 다음 ```var group = app.MapGroup("/user");``` 코드를 맨 첫 줄에 입력한다.

<img width="1026" height="372" alt="6  MapGroup" src="https://github.com/user-attachments/assets/9edee2dd-ce03-45e7-88e3-da31405ad72a" />

그리고 모든 엔드포인트 url에서 /user를 삭제한다.

그렇게 하면 최종적으로 다음과 같은 코드가 완성된다.  
엔드포인트 관리가 조금은 수월해질 것이다.

```C#
public static class UserEndpoint
{

    public static void RegisterUserApi(this WebApplication app)
    {
        var group = app.MapGroup("/user");

        group.MapGet("/{id}", async (UserDbContext db, IMapper mapper, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

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

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

            mapper.Map(updateUser, user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (UserDbContext db, int id) =>
        {
            User? user = await db.Users.FindAsync(id);

            if (user == null) return Results.NotFound("해당하는 id의 유저가 없습니다.");

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
```

## Program.cs에 등록

마지막으로 구현한 api들을 매핑하는 메서드가 구현된 RegisterUserApi() 메서드를 최상위 문에 추가해주기만 하면 된다.

```C#
var app = builder.Build();

app.RegisterUserApi();

app.Run();
```

최종적으로 최상위문에는 다음과 같이 설정 코드만 깔끔하게 남게 될 것이다.

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

이렇게 하면 미니멀 api의 간단한 api 구현이라는 장점을 유지하면서 최상위문이 복잡해지는 문제를 해결할 수 있다.

# 마무리

api 요청을 처리하는 메서드들을 엔드포인트별로 분류해보았다.
