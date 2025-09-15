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