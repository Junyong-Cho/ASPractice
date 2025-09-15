# ���� ó�� Middleware

api���� �� ��û�� ó���� �� ȯ�濡 ���� ���� ������ �߻��� �� �ִ�.  
db�� ������ �������� ���� ����ġ ���� ���ܻ�Ȳ�� ó���� �ִ� �ڵ尡 ������ ���ø����̼��� ����Ǵ� ���� ���� ����� ������ ������ ���̴�.

���� ���ܰ� �߻��ϸ� ���� ������ ���ܸ� ó�����־�� �Ѵ�.

�� api���� try catch������ ���ܸ� ó���ϵ��� �����ϱ⺸�� ��� api�� �ϳ��� Middleware ������ �����Ͽ� ���� �߻� �� Middleware���� ó���ϵ��� �����ϸ� �ڵ尡 �ξ� ���������� ���������� ��������.

ASP.NET Core�� �̷��� �̵��� �������ִ� ��ɱ����� ����ϰ� �ִ�. 

�̹� �ܰ迡���� �� Middleware�� �����Ͽ� api�� ���ܻ�Ȳ�� ó���غ����� �ϰڴ�.

## ���� �߻� ��Ȳ �����

�����ϰ� api���� ���ܸ� �߻����Ѻ��ڴ�.

```C#
app.MapGet("/error", () => 
{
    throw new Exception("Error! error!");
});
```

dotnet run(���� ���)���� ������ ������Ʈ���� �� url�� get ��û�� ������ ������ ���� ���� �������� Ȯ���� �� �ִµ� �� ��쵵 ASP.NET Core�� �̵��� ������ ���̴�.

![1. ���� �߻� Detail](../dummy/8%20����%20ó��%20middleware/1.%20����%20�߻�%20detail.png)

���� ��尡 �ƴ� � ���� �����ϸ�(���� �� ���� .exe ������ ������ ���) ������ ���� �������� ��Ÿ���� �̴� �� ���� ����� �������� �̾�����.

![2. ���� �߻� �](../dummy/8%20����%20ó��%20middleware/2.%20����%20�߻�%20�.png)

## 

���ܰ� �߻��ϴ��� ��Ŀ� �´� �����͸� �����ϵ��� �Ͽ� ���� ����� ������ �ּ�ȭ�ϴ� �ڵ带 �ۼ��غ� ���̴�.

������Ʈ�� Middlewares ���͸��� �����ϰ� �� �ȿ� ��� ���ܸ� ó���ϴ� GlobalExceptionHandlerMiddleware.cs ������ �����Ѵ�.

![3. ���͸� ����](../dummy/8%20����%20ó��%20middleware/3.%20���͸�%20����.png)

```C#
using System.Net;
using System.Text.Json;

namespace Middleware.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next; // ������ api ó�� �븮��

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;                       // �븮�� ����
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);               // api ��û ó��
        }
        catch (Exception ex)
        {
            await HandleException(context, ex); // ���� �߻���
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;  // ���� ���� ���� �ڵ�
        context.Response.ContentType = "application/json";                      // json ���·� ����

        var errorResponse = new                                                 // ������ json ����
        {
            context.Response.StatusCode,
            Message = "���� ����",
            Detail = ex.Message
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse);             // ������ ���� json ���·� ��ȯ

        return context.Response.WriteAsync(jsonResponse);                       // ���� ó��
    }
}
```

���� ���� �ڵ带 �ۼ��Ѵ�.

�ڵ带 ������ �������ڸ� ��û�� ������ ������ �޼��带 _next �븮�ڿ� �����ϰ� InvokeAsync �޼����� try catch������ api�� �����ϰ� ���ܰ� �߻��ϸ� HandlerException �޼���� json ���·� ���� �߻� ����� �����ϵ��� �Ѵ�.

�� �ڵ�� ��� api ���ο� try catch���� ���� �Ͱ� �����ϰ� �۵��ϴ� ���̴�.

## Middleware ����

���Ե� ������ �� �� �ִ�.

Program.cs ���Ͽ� WebApplication(app) ���� �ٷ� ������ ������ Middleware�� �߰��� �ش�.

```C#
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
```

�� �ڵ�� api���� �߻��ϴ� ��� ���� ��Ȳ�� �����ϵ��� �� ���̹Ƿ� ���� � Middleware�� WebApplication�� �߰��Ǵ��� �� ���� �ۼ��Ǿ�� �Ѵ�.

## �׽�Ʈ

���� �ٽ� /error url�� Get ��û�� �������ڴ�.

![4. ����ó�� �׽�Ʈ](../dummy/8%20����%20ó��%20middleware/4.%20����ó��%20�׽�Ʈ.png)

json ���·� ���ڰ� ���۵� ���� Ȯ���� �� ���� ���̴�.

# ������

���� ���� ó�� Middleware�� �߰��غ��Ҵ�.