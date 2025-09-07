# CRUD 구현

간단한 CRUD를 구현해 보겠다.  
C : Create  
R : Read  
U : Update  
D : Delete

Program.cs에 있는 모든 스크립트를 지운다.  
![초기화](../../dummy/2%20CRUD%20구현/초기화.png)

다음과 같은 초기 모습을 생성한다.
```
var builder = WebApplication.CreateBuilder();	// 웹 애플리케이션 빌더 인스턴스 생성

var app = builder.Build();			// 웹 애플리케이션 인스턴스 생성

// 이곳에 api 구현

app.Run();					// 웹 애플리케이션 실행
```

## Read 요청 구현(Read)

### 기초

api 구현 부분에 다음과 같은 Get 요청을 처리하는 api를 구현한다.
```
app.MapGet("/", () => "Hello World");
```

터미널을 열고 Program.cs 파일이 존재하는 경로로 이동하여 ```dotnet run``` 명령어를 실행해본다.  
![실행 주소](../../dummy/2%20CRUD%20구현/실행%20주소.png)

빌드가 완료되면 주소를 반환하는데 http://localhost:포트번호 부분을 복사하여 웹 브라우저에 입력해본다.  
![Hello World](../../dummy/2%20CRUD%20구현/hello%20world.png)  
api에 구현했던 문자열이 돌아온 것을 확인할 수 있다.

```
app.MapGet("/", () =>
{
	return "Hello World";
});
```
다음과 같이 구현해도 동일하게 작동한다.

### url을 통해 정보 받기

이 두 줄의 api를 추가한다.
```
app.MapGet("/{id}", (int id) => $"id is {id}");
// url에 명시된 변수와 람다식 매개변수명이 일치해야 함
app.MapGet("/search", (string q) => $"Results of {q}");
```

마찬가지로 build 후에 ```localhost:포트번호/(정수)```와 ```localhost:포트번호/search?q=(원하는 문자)```를 입력해보면 url에 입력된 정보에 따라 리턴값이 달라지는 것을 확인할 수 있다.  
![Id Search](../../dummy/2%20CRUD%20구현/id%20search.png)

### Json 형식 반환
솔루션 탐색기에서 프로젝트 이름을 오른쪽 클릭하고 추가>새 항목 추가를 선택하여 User.cs 파일을 생성한다.  
![항목 생성](../../dummy/2%20CRUD%20구현/항목%20생성.png)

자동으로 생성된 namespace를 현재 단계에서는 지우고
```
public record User(int Id, string Username, string Email);
```
다음과 같이 데이터 형식을 추가해본다.

그리고 Program.cs 파일에 다음과 같은 api를 추가한다.
```
User user = new(1234,"Hong GilDong", "GildongHong@example.com");

app.MapGet("/user", () => user);
```

빌드해서 /user url을 입력해서 확인해본다.  
![홍길동](../../dummy/2%20CRUD%20구현/홍길동.png)  
json 형태로 자동으로 변환되어 전달된 것을 확인할 수 있다.

### url에 명시된 정보를 기반으로 데이터 반환
유저 데이터가 저장되어 있는 리스트를 생성한다.
```
SortedList<int, User> users = new();
User user1 = new(1111, "Kim", "Kim@example.com");
User user2 = new(2222, "Park", "Park@example.com");
User user3 = new(3333, "Lee", "Lee@example.com");

users.Add(user1.Id, user1); users.Add(user2.Id, user2); users.Add(user3.Id, user3);
```
위 코드처럼 임시로 데이터를 저장하는 트리맵 자료구조를 구현한다.

그리고 다음과 같이 api를 구현하고 빌드 후 확인해본다.
```
app.MapGet("/user/all", () => 
{
    Return Results.Ok(users);
});
```
해당 url을 입력하면 저장된 모든 인스턴스가 json 형태로 반환되는 것을 확인할 수 있다.  
![모든 유저](../../../dummy/2%20CRUD%20구현/모든%20유저.png)


그리고 다음과 같이 url을 통해 id 정보를 입력받는 api를 구현하면
```
app.MapGet("user/{id}", (int id) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound(); // id가 없으면 404 반환

    return Results.Ok(users[id]);
});
```
id가 존재하는 유저의 json 데이터를 받환받을 수 있다.  
![유저 조회](../../dummy/2%20CRUD%20구현/유저%20조회.png)  

## Post 요청 구현(Create)
Get 요청 단계에서 만들었던 트리맵에 유저를 추가하는 요청을 처리하는 api를 만들어보겠다.

```
app.MapPost("/user/{id}", (User user, int id) =>            // Post 요청이므로 MapPost 메서드 사용
{
    if (users.ContainsKey(id)) return Results.Conflict();   // 이미 있는 아이디면 충돌

    users[id] = user;   // users.Add(id,user); 와 같음

    return Results.Created($"/user/{id}", user);    // 생성 성공
});
```

웹 브라우저 주소창에서는 Get 요청밖에 불가능하므로 api 요청을 보낼 새로운 프로젝트를 생성한다.
Visual Studio를 생성하고 첫 프로젝트를 생성할 때와 마찬가지로 새로운 프로젝트를 생성하면서 이번에는 콘솔 앱으로 프로젝트를 생성할 것이다.   
![콘솔 앱](../../../dummy/2%20CRUD%20구현/콘솔%20앱.png)  
![Request.Cs 생성](../../../dummy/2%20CRUD%20구현/Request.cs%20생성.png)

```
using System.Net.Http.Json;

HttpClient client = new();

var user = new
{
    Id = 1010,
    Username = "Choi",
    Email = "Choi@example.com"
};

var response = await client.PostAsJsonAsync($"http://localhost:(포트번호)/user/{user.Id}",user);

Console.WriteLine(response.StatusCode);
```

새로 생성한 프로젝트의 Program.cs 파일을 위와 같이 수정한 후 서버 프로젝트를 먼저 실행한 다음에 요청 프로젝트를 실행해본다.  
![Create](../../../dummy/2%20CRUD%20구현/create.png)  
다음과 같은 응답이 터미널에 도착하면 Post 요청이 성공한 것이다.

그리고 서버를 종료하지 않고 /user/1010(추가한 유저 아이디) 혹은 /user/all url을 웹브라우저에 입력해 보면  
![추가 유저 조회](../../../dummy/2%20CRUD%20구현/추가%20유저%20조회.png)  
유저가 새로 등록된 것을 확인할 수 있다.

## Put 요청 구현(Update)

Update에는 Put과 Patch가 있는데 간단하게 말하자면 Put은 전체 수정, Patch는 일부 수정이라고 할 수 있다.

User 인스턴스가 불변인 record로 선언되어 있기 때문에 해당 단계에서는 Put 요청만 구현하도록 할 텐데 Patch도 비슷하게 구현이 가능하다.

다음과 같은 코드를 추가한다.
```
app.MapPut("/user/{id}", (int id, User user) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id가 없으면 404 반환

    users[id] = user;   // 데이터 덮어쓰기

    return Results.NoContent();    // 성공했으나 반환할 데이터 없음
});
```

그리고 요청 프로젝트를 다음과 같이 수정한다.

```
var user = new
{
    Id = 1111,
    Username = "Hong",
    Email = "Hong@example.com"
};

var response = await client.PutAsJsonAsync($"http://localhost:5030/user/{user.Id}", user);

WriteLine(response.StatusCode);
```

Post 요청을 했을 때와 동일하게 수행했을 때 NoContent가 출력이 될 것이고  
/user/1111 url을 웹브라우저에 입력했을 때 Kim이었던 유저 데이터가 Hong으로 바뀐 것을 확인할 수 있을 것이다.

## Delete 요청 구현(Delete)

삭제 요청은 간단하다.  
삭제할 데이터의 아이디만 url에 추가해주면 삭제되도록 다음과 같은 코드를 작성한다.

```
app.MapDelete("/user/{id}", (int id) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id가 없으면 404 반환

    users.Remove(id);   // 데이터 삭제

    return Results.NoContent();    // 성공했으나 반환할 데이터 없음
});
```

마찬가지로 요청 프로젝트를 다음과 같이 수정한다.

```
var response = await client.DeleteAsync("http://localhost:5030/user/1111");

WriteLine(response.StatusCode);
```
테스트해보면 NoContent를 출력할 것이고 id가 1111인 데이터가 삭제된 것을 확인할 수 있을 것이다.


# 마무리
이렇게 CRUD를 간단하게 구현하는 방법을 정리해 보았다.