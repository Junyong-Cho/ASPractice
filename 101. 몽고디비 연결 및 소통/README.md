# ������ ����

���� �ܰ迡�� ������ DBMS ������ ��ġ�ϰ� admin ���� �������� ���־���.

���� ������� ������ �����غ��ڴ�.

## secrets.json

PostgreSQL�� ������ �־��� ��ó�� secrets.json�� ������ ���Ǵ� ConnectionString ���� ������ �� ���̴�.

```json
"MongoDbSettings" :{
	"ConnectionString": "mongodb://[��������]:[�н�����]@localhost:27017/[���� db]",
				   //ex) mongodb://admin:adminpassword@localhost:27017/admin
	"Database": "test",		// ����� db
	"Collection": "data"	// ����� �÷��� (���� DB�� �����̼�, ���̺�� ���� ����)
}
```

�׸��� nuget���� Mongodb�� �˻��Ͽ� MongoDB.Driver�� ��ġ�Ѵ�.

![1. ���������̹�](../.dummy/101%20����/1.%20���������̹�.png)

## ������ ���� ����

������ �����͸� �����ϱ� ���ؼ��� PostgreSQL�� ���������� ������ ������ �����ؾ� �Ѵ�.  
������ db�� ������� �����ϱ� ���ؼ� ������ ������ �����ؾ� �ϸ� ���� db�� ����ϴ� �Ͱ� ���̰� ���� �� ������ ���� db�� ������ ������ ����� ������ ���̱׷��̼��� �̿��Ͽ� �����̼� ������ ������ �־�� �ϴµ� ������� �ڵ常 �������ָ� �Ǵ� ���̰� �ִ�.

���� ������Ʈ�� Models/User.cs ��η� ������ �����Ѵ�.

![2. ����](../.dummy/101%20����/2.%20����.png)

User.cs ������ ���� PostgreSQL���� ������� ������ �Ȱ��� �ۼ��غ��ڴ�.

```C#
public class User
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
}
```

�׸��� ������ �°� �� �κп� �Ӽ��� �������ְڴ�.

```C#
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("id")]
    public string Id { get; set; } = null!;

    [BsonElement("name")]
    public string Username { get; set; } = null!;

    [BsonElement("user_id")]
    public string UserId { get; set; } = null!;

    [BsonElement("password")]
    public string Password { get; set; } = null!;

    [BsonElement("email")]
    public string Email { get; set; } = null!;
}
```

[BsonId]�� �������� �⺻Ű��� ���̰� �������� �⺻Ű�� Ÿ���� ObjectId Ÿ���ε� �̸� �������� string���� �ٷ�� �ͱ⿡ Id�� string���� �������־��� [BsonRepresentation(BsonType.ObjectId)] �Ӽ��� �߰����־���.  
�׸��� [BsonElement] �Ӽ��� �Ľ�Į ���̽��� �ʵ� �̸��� ������ũ ���̽��� ������ ����ǵ��� �����ϴ� �Ӽ��̴�.

## MongoDbSetting.cs

������ ������ ��������� ������ �����ϱ� ���� Ŭ������ ������ְڴ�.  
������Ʈ�� DbSettings/MongoDbSettings.cs ��η� ������ ������ش�.

![3. �������ñ���](../.dummy/101%20����/3.%20�������ñ���.png)

�׸��� MondoDbSettings.cs ������ ������ ������ ���� �ۼ����ش�.

```C#
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string Collection { get; set; } = null!;
}
```

ó���� secrets.json ���Ͽ� �߰����� MongoDbSettings�� Ű ���� �� �ڵ��� �ʵ� �̸��� �����ϰ� �������־�� �Ѵ�.
(ConnectionString, Database, Collection ��)

## MongoDbService.cs

�׸��� MongoDbSettings�� �̿��Ͽ� ������ �̿��ϴ� ���� Ŭ������ ������ �� ���̴�.  
������Ʈ�� Services/MongoDbService.cs ������ ������ش�.

![4. �����񼭺�](../.dummy/101%20����/4.%20�����񼭺�.png)

```C#
using Microsoft.Extensions.Options;
using Mongo.DbSettings;
using Mongo.Models;
using MongoDB.Driver;

public class MongoDbService
{
    private readonly IMongoCollection<User> _service;

    public MongoDbService(IOptions<MongoDbSettings> options)
    {
        _service = new MongoClient(options.Value.ConnectionString)
            .GetDatabase(options.Value.Database)
            .GetCollection<User>(options.Value.Collection);
        // ���� �Ʒ��� ���� �ڵ�
        MongoClient client = new MongoClient(options.Value.ConnectionString);
        IMongoDatabase db = client.GetDatabase(options.Value.Database);
        _service = db.GetCollection<User>(options.Value.Collection);
    }
}
```

�̷��� �����ϸ� MongoDbSetting���� ������ �� Db�� �װ��� �÷��ǿ��� �۾��� ���� �� �ִ�.

�׸��� �÷��ǿ��� �۾��� �ϱ� ���� ����, ����, ��ȸ ����� �ϴ� �޼��带 ������ ���� �߰��Ѵ�.

```C#
public class MongoDbService
{
    private readonly IMongoCollection<User> _service;

    public MongoDbService(IOptions<MongoDbSettings> options)
    {
        _service = new MongoClient(options.Value.ConnectionString)
            .GetDatabase(options.Value.Database)
            .GetCollection<User>(options.Value.Collection);
        // ���� �Ʒ��� ���� �ڵ�
        MongoClient client = new MongoClient(options.Value.ConnectionString);
        IMongoDatabase db = client.GetDatabase(options.Value.Database);
        _service = db.GetCollection<User>(options.Value.Collection);
    }
    // �߰�
    public async Task<List<User>> GetAllAsync() => await _service.Find(_ => true).ToListAsync();

    public async Task<User> GetAsync(string userId) => await _service.Find(u => u.UserId == userId).FirstAsync();

    public async Task PostAsync(User user) => await _service.InsertOneAsync(user);

    public async Task DeleteAsync(User user) => await _service.DeleteOneAsync(u => u.Id==user.Id);
}
```