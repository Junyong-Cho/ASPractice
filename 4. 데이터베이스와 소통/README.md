# Db와 소통하기

이전 단계에서 데이터베이스와 프로젝트를 연결해주었다.

이번 단계에서는 연결된 DB를 조작하여 CRUD를 구현해 보겠다.

그러나 그 전에 User.cs 파일의 User 데이터를 record로 설정해 주었는데 이렇게 설정하면 데이터를 수정할 때 기존 데이터를 지우고 새로운 데이터를 생성하는 식으로 진행할 수 밖에 없기에(즉 Patch가 불가능하기에) class로 바꾼 다음에 데이터베이스 구조를 갱신해보겠다.  
(사실 처음부터 class로 생성한 다음 db를 초기화해주면 되는데 실습을 위해서 record로 생성해 준 것이다.)

## User.cs 구조 변경

User.cs 파일을 다음과 같이 변경해 준다.

```C#
using System.ComponentModel.DataAnnotations;

namespace DbApi.Models;

//public record User(int Id, string Username, string Email);

public class User
{
    [Key]       // 기본키 속성
    public int Id { get; set; }
    [Required]  // 반드시 필요한 속성 == Not null
    public string? Username { get; set; }
    public string? Email { get; set; }
}
```

그리고 마이그레이션으로 db 구조를 갱신해 줄 것이다.

터미널에 ```dotnet ef migrations add UserRecordToClass``` 명령어를 입력한다.

![1. User 구조 변경](../../dummy/5%20DB%20api%20구현/1.%20User%20구조%20변경.png)

마이그레이션 추가가 성공하면 프로젝트의 migrations 디렉터리에 UserRecordToClass.cs 파일이 추가된 것을 확인할 수 있을 것이다.

![2. User Record To Class](../../dummy/5%20DB%20api%20구현/2.%20UserRecordToClass.png)

이렇게 db 구조 변경 이력이 확인할 수 있다. 그 다음 ```dotnet ef database update```명령어로 db 구조를 최종적으로 업데이트 한다.

## 비동기 api 구현

이제 요청이 들어오면 db에서 데이터를 찾아서 조작 후 처리하도록 할 텐데 그 전에 알아야 할 개념이 존재한다.  
C# 코드 내에서 관리되는 데이터는 요청 즉시 조작하고 전달하는 데 큰 시간이 들지 않았지만 db와 소통하게 된다면 db에서 데이터를 전달받는데 시간이 걸릴 수 있기 때문에 비동기적으로 동작하는 api를 구현해야 된다.  

만일 db에서 데이터를 요청하는 데 시간이 걸리면서 해당 줄에서 코드가 멈춰 있다면 그 뒤로 오는 요청도 그만큼 시간이 걸리게 된다.

따라서 db에 데이터 요청을 날려놓고 데이터가 올 동안 다른 요청을 처리할 수 있도록 하면 그만큼 시간을 절약하게 될 것이다. 이것이 바로 비동기적 프로그래밍이다.

물론 우리가 비동기적으로 동작하는 api를 구현하지 않는다 하더라도 동시에 여러 요청을 처리하지 못하는 것은 아니다.  
우리들 노트북도 그렇고 요즘 등장하는 cpu들은 기본적으로 여러 개의 코어와 여러 개의 쓰레드를 탑재하고 있기 때문에 하나의 쓰레드에서 db 요청을 기다리고 있더라도 다른 쓰레드에서 처리할 수 있도록 .NET Core가 구현을 잘 해 놓았기 때문이다.

다만 많은 클라이언트가 이용하는 서버로 대량의 요청이 들어오게 된다면 서버에 존재하는 코어와 쓰레드 수로는 어림도 없기 때문에 이것을 대비하여 비동기적으로 api를 구현해야 하는 것이다.

## 비동기 프로그래밍 예제

비동기 api를 구현하는 방법은 쉽다. 그냥 람다식 매개변수 앞에 async로 비동기 api라는 것을 명시해 주기만 하면 된다.  
그리고 async로 선언된(주로 Async()로 끝나는) 메서드로 데이터를 처리하도록 하고 그 앞에 await라고 명시해 주기만 하면 된다.  
이는 Async 메서드를 호출하고 해당 메서드의 작업이 완료될 때까지 해당 쓰레드를 비워두고 Async 메서드가 완료되면 다시 쓰레드로 돌아와 await에서 이어서 작업을 진행하겠다는 것을 의미한다.

다음과 같은 코드를 보자
```C#
app.MapGet("/", () => "index page");

app.MapGet("/sync", () =>
{
    Thread.Sleep(5000);         // 현재 메서드가 쓰레드를 차지한 채로 5초 대기

    return "sync";
});

app.MapGet("/async", async () =>
{

    await Task.Delay(5000);     // 현재 메서드를 백그라운드에 잠시 보내고 5초 후 불러옴

    return "async";
});
```

만약 /sync라는 url로 Get 요청을 100번 보내면 하나의 쓰레드를 쓴다고 가정하면 500초가 걸릴 것이다.
(요청 하나에 응답이 오기까지 5초가 걸리는데 응답이 오기 전에는 다른 작업을 못 하기 때문에)

그러나 /async하는 url로 Get 요청을 100번 보내면 일단 요청을 한 번에 100번 보내고 응답이 도착한 메서드 순서대로 작업을 처리할 것이기에 5초보다 조금 더 걸린다고 생각하면 된다.

그럼 한 번 이 서버에 100번 Get 요청을 하는 DOS 공격을 해보겠다.

```C#
HttpClient client = new();

string url = "http://localhost:5009/sync";      // 동기 url

List<Task<HttpResponseMessage>> tasks = new();

for (int i = 0; i < 100; i++)                   // 100번 동시 요청 Tasks 생성
    tasks.Add(client.GetAsync(url));

Console.WriteLine("\nStart\n");

await Task.WhenAll(tasks);                      // 동시 요청 시작

Console.WriteLine("\nEnd\n");
```

Dos 공격용 프로젝트를 만들어서 위와 같이 코드를 작성했다.

서버를 실행한 다음 위 코드를 실행한 다음에 웹 브라우저에 index 페이지 검색을 해보면 인덱스 페이지 로딩이 끝없이 이어지는 것을 확인할 수 있을 것이다.

```C#
HttpClient client = new();

//string url = "http://localhost:5009/sync";      // 동기 url
string url = "http://localhost:5009/async";     // 비동기 url

List<Task<HttpResponseMessage>> tasks = new();

for (int i = 0; i < 100; i++)                   // 100번 동시 요청 Tasks 생성
    tasks.Add(client.GetAsync(url));

Console.WriteLine("\nStart\n");

await Task.WhenAll(tasks);                      // 동시 요청 시작

Console.WriteLine("\nEnd\n");
```

이제 이 코드를 이렇게 수정해 보자.

위 코드를 실행한 다음에 index page를 요청해 웹 브라우저에 똑같이 검색을 하면 인덱스 페이지가 바로 반환되는 것을 확인할 수 있을 것이다.

## Get 구현

이제 첫 단계에서 구현했던 CRUD를 업그레이드해 보겠다.

```C#
app.MapGet("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);      // db에서 id에 맞는 User 데이터를 찾아올 동안 다른 작업 처리 가능하도록 함

    if (user == null) return Results.NotFound();

    return Results.Ok(user);
});

app.MapGet("/user/all", async (UserDbContext db) =>
{
    List<User> users = await db.users.ToListAsync();    // db에서 User 리스트를 가져오는 동안 다른 작업 처리 가능하도록 함

    return Results.Ok(users);
});
```
위와 같이 코드를 작성하면 클라이언트 측에서 '/user' url로 Get 요청을 보냈을 때 서버는 db 에서 데이터를 받을 때까지 기다리는 것이 아닌 db에 데이터를 요청한 다음에 데이터가 도착하기 전에 다른 요청도 처리할 수 있게 된다.
이후 db에서 데이터가 도착하면 await 이후의 코드를 실행하여 데이터를 전송하게 될 것이다.

이후 구현할 Post, Put, Patch, Delete도 마찬가지로 작성하면 된다.

## Post api 구현

```C#
app.MapPatch("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound(); // id가 없으면 404 반환

    return Results.NoContent();
});
```

## Put, Patch 구현

```C#
app.MapPut("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();        // id가 존재하지 않으면 404 반환

    db.Entry(user).CurrentValues.SetValues(updateUser); // 전체 데이터 수정

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPatch("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();        // id가 존재하지 않으면 404 반환

    if (updateUser.Username != null) user.Username = updateUser.Username;   // 수정해야 할 데이터가 있으면 수정 없으면 무시
    if (updateUser.Email != null) user.Email = updateUser.Email;

    await db.SaveChangesAsync();

    return Results.NoContent();
});
```

## Delete 구현

```C#
app.MapDelete("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();    // id가 존재하지 않으면 404 반환

    db.users.Remove(user);

    await db.SaveChangesAsync();

    return Results.NoContent();
});
```

## CRUD 요청해보기

이제 서버로 실제 요청을 보내볼 것이다.

Get 요청을 브라우저에서 확인해 보도록 하고 나머지는 Request 프로젝트를 만들어서 요청을 해보겠다.

## Post 요청

```C#
HttpClient client = new();

Write("id 입력>> ");
int id = int.Parse(ReadLine());
Write("이름 입력>> ");
string username = ReadLine();
string email = username + "@example.com";

var user = new
{
    Id = id,
    Username = username,
    Email = email
};

var response = await client.PostAsJsonAsync($"http://localhost:5009/user/{user.Id}", user);

WriteLine(response.StatusCode);
```

## Put 요청

```C#
Write("id 입력>> ");
int id = int.Parse(ReadLine());
Write("이름 입력>> ");
string username = ReadLine();
string email = username + "@example.com";

var user = new
{
    Id = id,
    Username = username,
    Email = email
};

var response = await client.PutAsJsonAsync($"http://localhost:5009/user/{user.Id}", user);

WriteLine(response.StatusCode);
```

## Patch 요청

```C#
Write("id 입력>> ");
int id = int.Parse(ReadLine());
Write("이름 변경 1/ 이메일 변경 2");
string username = null;
string email = null;

if (ReadLine() == "1")
    username = ReadLine();
else if (ReadLine() == "2")
    email = ReadLine() + "@example.com";

var user = new
{
    Id = id,
    Username = username,
    Email = email
};

var response = await client.PatchAsJsonAsync($"http://localhost:5009/user/{user.Id}", user);

WriteLine(response.StatusCode);
```

## Delete 요청

```C#
Write("삭제할 id 입력>> ");
int id = int.Parse(ReadLine());
var response = await client.DeleteAsync($"http://localhost:5009/user/{id}");

WriteLine(response.StatusCode);
```

## 실습

각 코드를 실행해 보면서 데이터가 제대로 저장되는지 확인해 본다.

한 번 예시를 보여주면 

![3. Post 요청](../../dummy/5%20DB%20api%20구현/3.%20Post%20요청.png)

Post 요청을 보내보았다. 

![4. Post 요청 확인](../../dummy/5%20DB%20api%20구현/4.%20Post%20요청%20확인.png)

db에 실제로 저장되었는지 확인해 볼 수도 있다.

터미널에 ```psql -U postgres```로 접속한 다음에
```\c mydb```로 db를 이동하고
```select * from users;``` 명령어를 입력해 보면 다음과 같이 데이터가 저장된 것을 확인할 수 있을 것이다.

![5. Db 확인](../../dummy/5%20DB%20api%20구현/5.%20Db%20확인.png)

db에 저장된 데이터를 삭제하고 싶으면 Delete 요청을 보낸 다음에

![6. Delete 요청](../../dummy/5%20DB%20api%20구현/6.%20Delete%20요청.png)

아까와 같이 확인해 보면 데이터가 사라진 것을 확인할 수 있다.

![7. 삭제 확인](../../dummy/5%20DB%20api%20구현/7.%20삭제%20확인.png)

## 마무리

Db와 연결된 서버에서 Db를 조작하는 api를 구현하고 비동기 프로그래밍을 실습해 보았으며 각 api마다 요청을 구현해 보았다.