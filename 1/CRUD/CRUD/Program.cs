var builder = WebApplication.CreateBuilder();   // 웹 애플리케이션 빌더 인스턴스 생성

var app = builder.Build();                      // 웹 애플리케이션 인스턴스 생성

// Get(Read)______________________________________________________________________________________________________  

app.MapGet("/", () => "Hello World");
//app.MapGet("/", () =>
//{
//    return "Hello World";
//});

app.MapGet("/{id}", (int id) => $"id is {id}");
// url에 명시된 변수와 람다식 매개변수명이 일치해야 함
app.MapGet("/search", (string q) => $"Results of {q}");

User user = new(1234,"Hong GilDong", "Gildonghong@example.com");
app.MapGet("/user", () => user);

SortedList<int, User> users = new();
User user1 = new(1111, "Kim", "Kim@example.com");
User user2 = new(2222, "Park", "Park@example.com");
User user3 = new(3333, "Lee", "Lee@example.com");

users.Add(user1.Id, user1); users.Add(user2.Id, user2); users.Add(user3.Id, user3);

app.MapGet("/user/all", () => 
{
    return Results.Ok(users);
});

app.MapGet("user/{id}", (int id) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id가 없으면 404 반환

    return Results.Ok(users[id]);
});


// Post(Create)___________________________________________________________________________________________________

app.MapPost("/user/{id}", (User user, int id) =>            // Post 요청이므로 MapPost 메서드 사용
{
    if (users.ContainsKey(id)) return Results.Conflict();   // 이미 있는 아이디면 충돌

    users[id] = user;   // users.Add(id,user); 와 같음

    return Results.Created($"/user/{id}", user);    // 생성 성공
});

//Put, Patch(Update)______________________________________________________________________________________________

app.MapPut("/user/{id}", (int id, User user) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id가 없으면 404 반환

    users[id] = user;   // 데이터 덮어쓰기

    return Results.NoContent();    // 성공했으나 반환할 데이터 없음
});

//Delete(Delete)__________________________________________________________________________________________________

app.MapDelete("/user/{id}", (int id) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id가 없으면 404 반환

    users.Remove(id);   // 데이터 삭제

    return Results.NoContent();    // 성공했으나 반환할 데이터 없음
});

app.Run();                                      // 웹 애플리케이션 실행