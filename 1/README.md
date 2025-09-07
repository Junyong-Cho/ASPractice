# CRUD ����

������ CRUD�� ������ ���ڴ�.  
C : Create  
R : Read  
U : Update  
D : Delete

Program.cs�� �ִ� ��� ��ũ��Ʈ�� �����.  
![�ʱ�ȭ](../../dummy/2%20CRUD%20����/�ʱ�ȭ.png)

������ ���� �ʱ� ����� �����Ѵ�.
```
var builder = WebApplication.CreateBuilder();	// �� ���ø����̼� ���� �ν��Ͻ� ����

var app = builder.Build();			// �� ���ø����̼� �ν��Ͻ� ����

// �̰��� api ����

app.Run();					// �� ���ø����̼� ����
```

## Read ��û ����(Read)

### ����

api ���� �κп� ������ ���� Get ��û�� ó���ϴ� api�� �����Ѵ�.
```
app.MapGet("/", () => "Hello World");
```

�͹̳��� ���� Program.cs ������ �����ϴ� ��η� �̵��Ͽ� ```dotnet run``` ��ɾ �����غ���.  
![���� �ּ�](../../dummy/2%20CRUD%20����/����%20�ּ�.png)

���尡 �Ϸ�Ǹ� �ּҸ� ��ȯ�ϴµ� http://localhost:��Ʈ��ȣ �κ��� �����Ͽ� �� �������� �Է��غ���.  
![Hello World](../../dummy/2%20CRUD%20����/hello%20world.png)  
api�� �����ߴ� ���ڿ��� ���ƿ� ���� Ȯ���� �� �ִ�.

```
app.MapGet("/", () =>
{
	return "Hello World";
});
```
������ ���� �����ص� �����ϰ� �۵��Ѵ�.

### url�� ���� ���� �ޱ�

�� �� ���� api�� �߰��Ѵ�.
```
app.MapGet("/{id}", (int id) => $"id is {id}");
// url�� ��õ� ������ ���ٽ� �Ű��������� ��ġ�ؾ� ��
app.MapGet("/search", (string q) => $"Results of {q}");
```

���������� build �Ŀ� ```localhost:��Ʈ��ȣ/(����)```�� ```localhost:��Ʈ��ȣ/search?q=(���ϴ� ����)```�� �Է��غ��� url�� �Էµ� ������ ���� ���ϰ��� �޶����� ���� Ȯ���� �� �ִ�.  
![Id Search](../../dummy/2%20CRUD%20����/id%20search.png)

### Json ���� ��ȯ
�ַ�� Ž���⿡�� ������Ʈ �̸��� ������ Ŭ���ϰ� �߰�>�� �׸� �߰��� �����Ͽ� User.cs ������ �����Ѵ�.  
![�׸� ����](../../dummy/2%20CRUD%20����/�׸�%20����.png)

�ڵ����� ������ namespace�� ���� �ܰ迡���� �����
```
public record User(int Id, string Username, string Email);
```
������ ���� ������ ������ �߰��غ���.

�׸��� Program.cs ���Ͽ� ������ ���� api�� �߰��Ѵ�.
```
User user = new(1234,"Hong GilDong", "GildongHong@example.com");

app.MapGet("/user", () => user);
```

�����ؼ� /user url�� �Է��ؼ� Ȯ���غ���.  
![ȫ�浿](../../dummy/2%20CRUD%20����/ȫ�浿.png)  
json ���·� �ڵ����� ��ȯ�Ǿ� ���޵� ���� Ȯ���� �� �ִ�.

### url�� ��õ� ������ ������� ������ ��ȯ
���� �����Ͱ� ����Ǿ� �ִ� ����Ʈ�� �����Ѵ�.
```
SortedList<int, User> users = new();
User user1 = new(1111, "Kim", "Kim@example.com");
User user2 = new(2222, "Park", "Park@example.com");
User user3 = new(3333, "Lee", "Lee@example.com");

users.Add(user1.Id, user1); users.Add(user2.Id, user2); users.Add(user3.Id, user3);
```
�� �ڵ�ó�� �ӽ÷� �����͸� �����ϴ� Ʈ���� �ڷᱸ���� �����Ѵ�.

�׸��� ������ ���� api�� �����ϰ� ���� �� Ȯ���غ���.
```
app.MapGet("/user/all", () => 
{
    Return Results.Ok(users);
});
```
�ش� url�� �Է��ϸ� ����� ��� �ν��Ͻ��� json ���·� ��ȯ�Ǵ� ���� Ȯ���� �� �ִ�.  
![��� ����](../../../dummy/2%20CRUD%20����/���%20����.png)


�׸��� ������ ���� url�� ���� id ������ �Է¹޴� api�� �����ϸ�
```
app.MapGet("user/{id}", (int id) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound(); // id�� ������ 404 ��ȯ

    return Results.Ok(users[id]);
});
```
id�� �����ϴ� ������ json �����͸� ��ȯ���� �� �ִ�.  
![���� ��ȸ](../../dummy/2%20CRUD%20����/����%20��ȸ.png)  

## Post ��û ����(Create)
Get ��û �ܰ迡�� ������� Ʈ���ʿ� ������ �߰��ϴ� ��û�� ó���ϴ� api�� �����ڴ�.

```
app.MapPost("/user/{id}", (User user, int id) =>            // Post ��û�̹Ƿ� MapPost �޼��� ���
{
    if (users.ContainsKey(id)) return Results.Conflict();   // �̹� �ִ� ���̵�� �浹

    users[id] = user;   // users.Add(id,user); �� ����

    return Results.Created($"/user/{id}", user);    // ���� ����
});
```

�� ������ �ּ�â������ Get ��û�ۿ� �Ұ����ϹǷ� api ��û�� ���� ���ο� ������Ʈ�� �����Ѵ�.
Visual Studio�� �����ϰ� ù ������Ʈ�� ������ ���� ���������� ���ο� ������Ʈ�� �����ϸ鼭 �̹����� �ܼ� ������ ������Ʈ�� ������ ���̴�.   
![�ܼ� ��](../../../dummy/2%20CRUD%20����/�ܼ�%20��.png)  
![Request.Cs ����](../../../dummy/2%20CRUD%20����/Request.cs%20����.png)

```
using System.Net.Http.Json;

HttpClient client = new();

var user = new
{
    Id = 1010,
    Username = "Choi",
    Email = "Choi@example.com"
};

var response = await client.PostAsJsonAsync($"http://localhost:(��Ʈ��ȣ)/user/{user.Id}",user);

Console.WriteLine(response.StatusCode);
```

���� ������ ������Ʈ�� Program.cs ������ ���� ���� ������ �� ���� ������Ʈ�� ���� ������ ������ ��û ������Ʈ�� �����غ���.  
![Create](../../../dummy/2%20CRUD%20����/create.png)  
������ ���� ������ �͹̳ο� �����ϸ� Post ��û�� ������ ���̴�.

�׸��� ������ �������� �ʰ� /user/1010(�߰��� ���� ���̵�) Ȥ�� /user/all url�� ���������� �Է��� ����  
![�߰� ���� ��ȸ](../../../dummy/2%20CRUD%20����/�߰�%20����%20��ȸ.png)  
������ ���� ��ϵ� ���� Ȯ���� �� �ִ�.

## Put ��û ����(Update)

Update���� Put�� Patch�� �ִµ� �����ϰ� �����ڸ� Put�� ��ü ����, Patch�� �Ϻ� �����̶�� �� �� �ִ�.

User �ν��Ͻ��� �Һ��� record�� ����Ǿ� �ֱ� ������ �ش� �ܰ迡���� Put ��û�� �����ϵ��� �� �ٵ� Patch�� ����ϰ� ������ �����ϴ�.

������ ���� �ڵ带 �߰��Ѵ�.
```
app.MapPut("/user/{id}", (int id, User user) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id�� ������ 404 ��ȯ

    users[id] = user;   // ������ �����

    return Results.NoContent();    // ���������� ��ȯ�� ������ ����
});
```

�׸��� ��û ������Ʈ�� ������ ���� �����Ѵ�.

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

Post ��û�� ���� ���� �����ϰ� �������� �� NoContent�� ����� �� ���̰�  
/user/1111 url�� ���������� �Է����� �� Kim�̾��� ���� �����Ͱ� Hong���� �ٲ� ���� Ȯ���� �� ���� ���̴�.

## Delete ��û ����(Delete)

���� ��û�� �����ϴ�.  
������ �������� ���̵� url�� �߰����ָ� �����ǵ��� ������ ���� �ڵ带 �ۼ��Ѵ�.

```
app.MapDelete("/user/{id}", (int id) =>
{
    if (!users.ContainsKey(id)) return Results.NotFound();  // id�� ������ 404 ��ȯ

    users.Remove(id);   // ������ ����

    return Results.NoContent();    // ���������� ��ȯ�� ������ ����
});
```

���������� ��û ������Ʈ�� ������ ���� �����Ѵ�.

```
var response = await client.DeleteAsync("http://localhost:5030/user/1111");

WriteLine(response.StatusCode);
```
�׽�Ʈ�غ��� NoContent�� ����� ���̰� id�� 1111�� �����Ͱ� ������ ���� Ȯ���� �� ���� ���̴�.


# ������
�̷��� CRUD�� �����ϰ� �����ϴ� ����� ������ ���Ҵ�.