var builder = WebApplication.CreateBuilder();   // �� ���ø����̼� ���� �ν��Ͻ� ����

var app = builder.Build();                      // �� ���ø����̼� �ν��Ͻ� ����

// Get(Read)______________________________________________________________________________________________________  

app.MapGet("/", () => "Hello World");
//app.MapGet("/", () =>
//{
//    return "Hello World";
//});

app.MapGet("/{id}", (int id) => $"id is {id}");
// url�� ��õ� ������ ���ٽ� �Ű��������� ��ġ�ؾ� ��
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
    if (!users.ContainsKey(id)) return Results.NotFound();  // id�� ������ 404 ��ȯ

    return Results.Ok(users[id]);
});


// Post(Create)___________________________________________________________________________________________________

app.MapPost("/user/{id}", (User user, int id) =>            // Post ��û�̹Ƿ� MapPost �޼��� ���
{
    if (users.ContainsKey(id)) return Results.Conflict();   // �̹� �ִ� ���̵�� �浹

    users[id] = user;   // users.Add(id,user); �� ����

    return Results.Created($"/user/{id}", user);    // ���� ����
});

//Put, Patch(Update)______________________________________________________________________________________________

app.MapPut("/user/{id}", (int id, User user) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id�� ������ 404 ��ȯ

    users[id] = user;   // ������ �����

    return Results.NoContent();    // ���������� ��ȯ�� ������ ����
});

//Delete(Delete)__________________________________________________________________________________________________

app.MapDelete("/user/{id}", (int id) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id�� ������ 404 ��ȯ

    users.Remove(id);   // ������ ����

    return Results.NoContent();    // ���������� ��ȯ�� ������ ����
});

app.Run();                                      // �� ���ø����̼� ����