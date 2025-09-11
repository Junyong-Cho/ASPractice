# 유효성 검사

이제 Db에 저장할 데이터의 유효성을 검사하는 코드를 추가해보겠다.  
[Key], [Required] 같이 데이터에 속성을 부여해 주었지만 이는 db의 데이터 구조를 정의할 때 쓰이며 데이터를 생성할 때에는 해당 데이터가 Null인지 이메일 형식인지 즉, 유효한 형식인지 검사해주지 않는다.  
데이터의 유효성을 검사하지 않고 Db에 저장을 시도하는 경우에 db에서 호출한 서버 측에 예외를 던지게 되며 서버 측에서 예외를 처리하기 위한 코드를 추가해 주어야 한다.  
try catch문으로 예외를 처리해 주어도 되지만 db에 데이터를 보내고 예외를 응답받는 상황은 이상적이지 않다. (db에게 데이터를 전달하기 전에 유효성을 검사하는 것이 좋다.)  
그러므로 서버 측에서 먼저 데이터의 유효성을 검사해 주는 것이 좋다.

## FluentValidation 라이브러리
유효성을 검사해주는 라이브러리를 설치해보겠다. nuget에 FluentValidation(validator와 헷갈리면 안 된다.)을 검색한다.

![1. Fluent Validation](../dummy/6%20유효성%20검사/1.%20FluentValidation.png)

FluentValidation과 밑에 있는 DependencyInjectionExtensions 확장팩까지 같이 설치해 준다.

```dotnet list package```로 설치를 확인한다.

## UserValidator 구현
설치가 완료되면 User 데이터의 유효성을 검사해 줄 Validator를 구현해 줄 것이다.

프로젝트에 Validators 디렉터리를 생성하고 그 밑으로 UserValidator.cs 파일을 생성해 준다.

![2. 프로젝트 구조](../dummy/6%20유효성%20검사/2.%20프로젝트%20구조.png)

UserValidator.cs 파일을 다음과 같이 작성한다.

```C#
using FluentValidation;
using Validation.Models;

namespace Validation.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username).NotEmpty().WithMessage("필수 입력");
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("필수 입력")
            .Length(8,30).WithMessage("8 이상 30 이하 문자")
            .Matches(@"[A-Z]").WithMessage("대문자 포함")
            .Matches(@"[a-z]").WithMessage("소문자 포함")
            .Matches(@"[0-9]").WithMessage("숫자 포함")
            .Matches(@"[!@#$,.*]").WithMessage("지정된 특수문자 포함");
        RuleFor(u => u.Email).EmailAddress().WithMessage("올바르지 않은 이메일 형식");
    }
}
```

유효성을 검사하고 싶은 속성을 NotEmpty EmailAddress Length Matches등으로 설정해 준다.

## api에서 검사

Post, Put, Patch 단계에서 전달 받은 데이터를 저장 혹은 수정하긴 전에 유효성 검사 코드를 작성해 준다.

```C#
app.MapPost("/user/{id}",async (UserDbContext db, IValidator<User> validator, User user, int id) =>
{
    var result = await validator.ValidateAsync(user);                           // 유효성 검사
    
    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());                // 유효성에 문제가 있으면 메세지와 함께 반환

    if (await db.users.AnyAsync(u => u.Id == id)) return Results.Conflict();    // id 존재 확인

    /*
    .....
    ..... db 저장 코드 작성
    */
    return Results.Created($"/user/{id}", user);                                // 정상 작동
});
```

Put도 Post와 마찬가지로 유효성을 검사하는 코드를 db에 요청하기 전에 작성을 해 주면 되는데 Patch는 수정하지 않는 데이터를 null로 비워두기 때문에 UserValidator가 아닌 PatchUserDtoValidator와 같은 Validator를 생성해 주어야 한다.

DTO(Data Transfer Object)는 다음 단계에 할 것이므로 PatchValidator는 다음 단계에 구현해 보도록 하겠다.

# 마무리

유효성 검사 기능을 하는 코드를 추가해보았다.