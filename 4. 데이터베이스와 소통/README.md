# Db�� �����ϱ�

���� �ܰ迡�� �����ͺ��̽��� ������Ʈ�� �������־���.

�̹� �ܰ迡���� ����� DB�� �����Ͽ� CRUD�� ������ ���ڴ�.

�׷��� �� ���� User.cs ������ User �����͸� record�� ������ �־��µ� �̷��� �����ϸ� �����͸� ������ �� ���� �����͸� ����� ���ο� �����͸� �����ϴ� ������ ������ �� �ۿ� ���⿡(�� Patch�� �Ұ����ϱ⿡) class�� �ٲ� ������ �����ͺ��̽� ������ �����غ��ڴ�.  
(��� ó������ class�� ������ ���� db�� �ʱ�ȭ���ָ� �Ǵµ� �ǽ��� ���ؼ� record�� ������ �� ���̴�.)

## User.cs ���� ����

User.cs ������ ������ ���� ������ �ش�.

```C#
using System.ComponentModel.DataAnnotations;

namespace DbApi.Models;

//public record User(int Id, string Username, string Email);

public class User
{
    [Key]       // �⺻Ű �Ӽ�
    public int Id { get; set; }
    [Required]  // �ݵ�� �ʿ��� �Ӽ� == Not null
    public string? Username { get; set; }
    public string? Email { get; set; }
}
```

�׸��� ���̱׷��̼����� db ������ ������ �� ���̴�.

�͹̳ο� ```dotnet ef migrations add UserRecordToClass``` ��ɾ �Է��Ѵ�.

![1. User ���� ����](../../dummy/5%20DB%20api%20����/1.%20User%20����%20����.png)

���̱׷��̼� �߰��� �����ϸ� ������Ʈ�� migrations ���͸��� UserRecordToClass.cs ������ �߰��� ���� Ȯ���� �� ���� ���̴�.

![2. User Record To Class](../../dummy/5%20DB%20api%20����/2.%20UserRecordToClass.png)

�̷��� db ���� ���� �̷��� Ȯ���� �� �ִ�. �� ���� ```dotnet ef database update```��ɾ�� db ������ ���������� ������Ʈ �Ѵ�.

## �񵿱� api ����

���� ��û�� ������ db���� �����͸� ã�Ƽ� ���� �� ó���ϵ��� �� �ٵ� �� ���� �˾ƾ� �� ������ �����Ѵ�.  
C# �ڵ� ������ �����Ǵ� �����ʹ� ��û ��� �����ϰ� �����ϴ� �� ū �ð��� ���� �ʾ����� db�� �����ϰ� �ȴٸ� db���� �����͸� ���޹޴µ� �ð��� �ɸ� �� �ֱ� ������ �񵿱������� �����ϴ� api�� �����ؾ� �ȴ�.  

���� db���� �����͸� ��û�ϴ� �� �ð��� �ɸ��鼭 �ش� �ٿ��� �ڵ尡 ���� �ִٸ� �� �ڷ� ���� ��û�� �׸�ŭ �ð��� �ɸ��� �ȴ�.

���� db�� ������ ��û�� �������� �����Ͱ� �� ���� �ٸ� ��û�� ó���� �� �ֵ��� �ϸ� �׸�ŭ �ð��� �����ϰ� �� ���̴�. �̰��� �ٷ� �񵿱��� ���α׷����̴�.

���� �츮�� �񵿱������� �����ϴ� api�� �������� �ʴ´� �ϴ��� ���ÿ� ���� ��û�� ó������ ���ϴ� ���� �ƴϴ�.  
�츮�� ��Ʈ�ϵ� �׷��� ���� �����ϴ� cpu���� �⺻������ ���� ���� �ھ�� ���� ���� �����带 ž���ϰ� �ֱ� ������ �ϳ��� �����忡�� db ��û�� ��ٸ��� �ִ��� �ٸ� �����忡�� ó���� �� �ֵ��� .NET Core�� ������ �� �� ���ұ� �����̴�.

�ٸ� ���� Ŭ���̾�Ʈ�� �̿��ϴ� ������ �뷮�� ��û�� ������ �ȴٸ� ������ �����ϴ� �ھ�� ������ ���δ� ��� ���� ������ �̰��� ����Ͽ� �񵿱������� api�� �����ؾ� �ϴ� ���̴�.

## �񵿱� ���α׷��� ����

�񵿱� api�� �����ϴ� ����� ����. �׳� ���ٽ� �Ű����� �տ� async�� �񵿱� api��� ���� ����� �ֱ⸸ �ϸ� �ȴ�.  
�׸��� async�� �����(�ַ� Async()�� ������) �޼���� �����͸� ó���ϵ��� �ϰ� �� �տ� await��� ����� �ֱ⸸ �ϸ� �ȴ�.  
�̴� Async �޼��带 ȣ���ϰ� �ش� �޼����� �۾��� �Ϸ�� ������ �ش� �����带 ����ΰ� Async �޼��尡 �Ϸ�Ǹ� �ٽ� ������� ���ƿ� await���� �̾ �۾��� �����ϰڴٴ� ���� �ǹ��Ѵ�.

������ ���� �ڵ带 ����
```C#
app.MapGet("/", () => "index page");

app.MapGet("/sync", () =>
{
    Thread.Sleep(5000);         // ���� �޼��尡 �����带 ������ ä�� 5�� ���

    return "sync";
});

app.MapGet("/async", async () =>
{

    await Task.Delay(5000);     // ���� �޼��带 ��׶��忡 ��� ������ 5�� �� �ҷ���

    return "async";
});
```

���� /sync��� url�� Get ��û�� 100�� ������ �ϳ��� �����带 ���ٰ� �����ϸ� 500�ʰ� �ɸ� ���̴�.
(��û �ϳ��� ������ ������� 5�ʰ� �ɸ��µ� ������ ���� ������ �ٸ� �۾��� �� �ϱ� ������)

�׷��� /async�ϴ� url�� Get ��û�� 100�� ������ �ϴ� ��û�� �� ���� 100�� ������ ������ ������ �޼��� ������� �۾��� ó���� ���̱⿡ 5�ʺ��� ���� �� �ɸ��ٰ� �����ϸ� �ȴ�.

�׷� �� �� �� ������ 100�� Get ��û�� �ϴ� DOS ������ �غ��ڴ�.

```C#
HttpClient client = new();

string url = "http://localhost:5009/sync";      // ���� url

List<Task<HttpResponseMessage>> tasks = new();

for (int i = 0; i < 100; i++)                   // 100�� ���� ��û Tasks ����
    tasks.Add(client.GetAsync(url));

Console.WriteLine("\nStart\n");

await Task.WhenAll(tasks);                      // ���� ��û ����

Console.WriteLine("\nEnd\n");
```

Dos ���ݿ� ������Ʈ�� ���� ���� ���� �ڵ带 �ۼ��ߴ�.

������ ������ ���� �� �ڵ带 ������ ������ �� �������� index ������ �˻��� �غ��� �ε��� ������ �ε��� ������ �̾����� ���� Ȯ���� �� ���� ���̴�.

```C#
HttpClient client = new();

//string url = "http://localhost:5009/sync";      // ���� url
string url = "http://localhost:5009/async";     // �񵿱� url

List<Task<HttpResponseMessage>> tasks = new();

for (int i = 0; i < 100; i++)                   // 100�� ���� ��û Tasks ����
    tasks.Add(client.GetAsync(url));

Console.WriteLine("\nStart\n");

await Task.WhenAll(tasks);                      // ���� ��û ����

Console.WriteLine("\nEnd\n");
```

���� �� �ڵ带 �̷��� ������ ����.

�� �ڵ带 ������ ������ index page�� ��û�� �� �������� �Ȱ��� �˻��� �ϸ� �ε��� �������� �ٷ� ��ȯ�Ǵ� ���� Ȯ���� �� ���� ���̴�.

## Get ����

���� ù �ܰ迡�� �����ߴ� CRUD�� ���׷��̵��� ���ڴ�.

```C#
app.MapGet("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);      // db���� id�� �´� User �����͸� ã�ƿ� ���� �ٸ� �۾� ó�� �����ϵ��� ��

    if (user == null) return Results.NotFound();

    return Results.Ok(user);
});

app.MapGet("/user/all", async (UserDbContext db) =>
{
    List<User> users = await db.users.ToListAsync();    // db���� User ����Ʈ�� �������� ���� �ٸ� �۾� ó�� �����ϵ��� ��

    return Results.Ok(users);
});
```
���� ���� �ڵ带 �ۼ��ϸ� Ŭ���̾�Ʈ ������ '/user' url�� Get ��û�� ������ �� ������ db ���� �����͸� ���� ������ ��ٸ��� ���� �ƴ� db�� �����͸� ��û�� ������ �����Ͱ� �����ϱ� ���� �ٸ� ��û�� ó���� �� �ְ� �ȴ�.
���� db���� �����Ͱ� �����ϸ� await ������ �ڵ带 �����Ͽ� �����͸� �����ϰ� �� ���̴�.

���� ������ Post, Put, Patch, Delete�� ���������� �ۼ��ϸ� �ȴ�.

## Post api ����

```C#
app.MapPatch("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound(); // id�� ������ 404 ��ȯ

    return Results.NoContent();
});
```

## Put, Patch ����

```C#
app.MapPut("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();        // id�� �������� ������ 404 ��ȯ

    db.Entry(user).CurrentValues.SetValues(updateUser); // ��ü ������ ����

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPatch("/user/{id}", async (UserDbContext db, User updateUser, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();        // id�� �������� ������ 404 ��ȯ

    if (updateUser.Username != null) user.Username = updateUser.Username;   // �����ؾ� �� �����Ͱ� ������ ���� ������ ����
    if (updateUser.Email != null) user.Email = updateUser.Email;

    await db.SaveChangesAsync();

    return Results.NoContent();
});
```

## Delete ����

```C#
app.MapDelete("/user/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.users.FindAsync(id);

    if (user == null) return Results.NotFound();    // id�� �������� ������ 404 ��ȯ

    db.users.Remove(user);

    await db.SaveChangesAsync();

    return Results.NoContent();
});
```

## CRUD ��û�غ���

���� ������ ���� ��û�� ������ ���̴�.

Get ��û�� ���������� Ȯ���� ������ �ϰ� �������� Request ������Ʈ�� ���� ��û�� �غ��ڴ�.

## Post ��û

```C#
HttpClient client = new();

Write("id �Է�>> ");
int id = int.Parse(ReadLine());
Write("�̸� �Է�>> ");
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

## Put ��û

```C#
Write("id �Է�>> ");
int id = int.Parse(ReadLine());
Write("�̸� �Է�>> ");
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

## Patch ��û

```C#
Write("id �Է�>> ");
int id = int.Parse(ReadLine());
Write("�̸� ���� 1/ �̸��� ���� 2");
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

## Delete ��û

```C#
Write("������ id �Է�>> ");
int id = int.Parse(ReadLine());
var response = await client.DeleteAsync($"http://localhost:5009/user/{id}");

WriteLine(response.StatusCode);
```

## �ǽ�

�� �ڵ带 ������ ���鼭 �����Ͱ� ����� ����Ǵ��� Ȯ���� ����.

�� �� ���ø� �����ָ� 

![3. Post ��û](../../dummy/5%20DB%20api%20����/3.%20Post%20��û.png)

Post ��û�� �������Ҵ�. 

![4. Post ��û Ȯ��](../../dummy/5%20DB%20api%20����/4.%20Post%20��û%20Ȯ��.png)

db�� ������ ����Ǿ����� Ȯ���� �� ���� �ִ�.

�͹̳ο� ```psql -U postgres```�� ������ ������
```\c mydb```�� db�� �̵��ϰ�
```select * from users;``` ��ɾ �Է��� ���� ������ ���� �����Ͱ� ����� ���� Ȯ���� �� ���� ���̴�.

![5. Db Ȯ��](../../dummy/5%20DB%20api%20����/5.%20Db%20Ȯ��.png)

db�� ����� �����͸� �����ϰ� ������ Delete ��û�� ���� ������

![6. Delete ��û](../../dummy/5%20DB%20api%20����/6.%20Delete%20��û.png)

�Ʊ�� ���� Ȯ���� ���� �����Ͱ� ����� ���� Ȯ���� �� �ִ�.

![7. ���� Ȯ��](../../dummy/5%20DB%20api%20����/7.%20����%20Ȯ��.png)

## ������

Db�� ����� �������� Db�� �����ϴ� api�� �����ϰ� �񵿱� ���α׷����� �ǽ��� �������� �� api���� ��û�� ������ ���Ҵ�.