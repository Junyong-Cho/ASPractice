# 예외 처리 Middleware

api에서 각 요청을 처리할 때 환경에 따라 여러 오류가 발생할 수 있다.  
db와 연결이 끊어지는 등의 예상치 못한 예외상황을 처리해 주는 코드가 없으면 애플리케이션이 종료되는 등의 나쁜 사용자 경험을 제공할 것이다.

따라서 예외가 발생하면 서버 측에서 예외를 처리해주어야 한다.

각 api에서 try catch문으로 예외를 처리하도록 구현하기보다 모든 api를 하나의 Middleware 위에서 구동하여 예외 발생 시 Middleware에서 처리하도록 구현하면 코드가 훨씬 간결해지고 유지보수가 편해진다.

ASP.NET Core는 이러한 미들웨어를 주입해주는 기능까지도 겸비하고 있다. 

이번 단계에서는 이 Middleware를 구현하여 api의 예외상황을 처리해보도록 하겠다.

## 예외 발생 상황 만들기

간단하게 api에서 예외를 발생시켜보겠다.

```C#
app.MapGet("/error", () => 
{
    throw new Exception("Error! error!");
});
```

dotnet run(개발 모드)으로 실행한 프로젝트에서 위 url로 get 요청을 보내면 다음과 같은 예외 페이지를 확인할 수 있는데 이 경우도 ASP.NET Core의 미들웨어가 동작한 것이다.

![1. 예외 발생 Detail](../dummy/8%20예외%20처리%20middleware/1.%20예외%20발생%20detail.png)

개발 모드가 아닌 운영 모드로 실행하면(빌드 후 직접 .exe 파일을 실행한 경우) 다음과 같은 페이지가 나타나며 이는 곧 나쁜 사용자 경험으로 이어진다.

![2. 예외 발생 운영](../dummy/8%20예외%20처리%20middleware/2.%20예외%20발생%20운영.png)

## 

예외가 발생하더라도 양식에 맞는 데이터를 전송하도록 하여 나쁜 사용자 경험을 최소화하는 코드를 작성해볼 것이다.

프로젝트에 Middlewares 디렉터리를 생성하고 그 안에 모든 예외를 처리하는 GlobalExceptionHandlerMiddleware.cs 파일을 생성한다.

![3. 디렉터리 구조](../dummy/8%20예외%20처리%20middleware/3.%20디렉터리%20구조.png)

```C#
using System.Net;
using System.Text.Json;

namespace Middleware.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next; // 실행할 api 처리 대리자

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;                       // 대리자 지정
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);               // api 요청 처리
        }
        catch (Exception ex)
        {
            await HandleException(context, ex); // 예외 발생시
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;  // 서버 에러 상태 코드
        context.Response.ContentType = "application/json";                      // json 형태로 전달

        var errorResponse = new                                                 // 전달할 json 형태
        {
            context.Response.StatusCode,
            Message = "서버 에러",
            Detail = ex.Message
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse);             // 데이터 형식 json 형태로 변환

        return context.Response.WriteAsync(jsonResponse);                       // 예외 처리
    }
}
```

위와 같이 코드를 작성한다.

코드를 간단히 설명하자면 요청이 들어오면 실행할 메서드를 _next 대리자에 저장하고 InvokeAsync 메서드의 try catch문에서 api를 실행하고 예외가 발생하면 HandlerException 메서드로 json 형태로 예외 발생 사실을 전달하도록 한다.

이 코드는 모든 api 내부에 try catch문을 씌운 것과 동일하게 작동하는 것이다.

## Middleware 주입

주입도 간단히 할 수 있다.

Program.cs 파일에 WebApplication(app) 빌드 바로 다음에 생성한 Middleware를 추가해 준다.

```C#
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
```

이 코드는 api에서 발생하는 모든 예외 상황을 관리하도록 할 것이므로 이후 어떤 Middleware가 WebApplication에 추가되더라도 그 위에 작성되어야 한다.

## 테스트

이제 다시 /error url로 Get 요청을 보내보겠다.

![4. 예외처리 테스트](../dummy/8%20예외%20처리%20middleware/4.%20예외처리%20테스트.png)

json 형태로 예쁘게 전송된 것을 확인할 수 있을 것이다.

# 마무리

전역 예외 처리 Middleware를 추가해보았다.