# 몽고디비 연결

이전 단계에서 비정형 DBMS 몽고디비를 설치하고 admin 유저 생성까지 해주었다.

이제 몽고디비와 서버를 연결해보겠다.

## secrets.json

PostgreSQL을 연결해 주었을 때처럼 secrets.json에 몽고디비에 사용되는 ConnectionString 등을 설정해 줄 것이다.

```json
"MongoDbSettings" :{
	"ConnectionString": "mongodb://[유저네임]:[패스워드]@localhost:27017/[인증 db]",
				   //ex) mongodb://admin:adminpassword@localhost:27017/admin
	"Database": "test",		// 사용할 db
	"Collection": "data"	// 사용할 컬렉션 (정형 DB의 릴레이션, 테이블과 같은 개념)
}
```

그리고 nuget에서 Mongodb를 검색하여 MongoDB.Driver를 설치한다.

![1. 몽고디비드라이버](../.dummy/101%20몽고/1.%20몽고디비드라이버.png)

## 데이터 구조 정의

몽고디비에 데이터를 저장하기 위해서는 PostgreSQL과 마찬가지로 데이터 구조를 정의해야 한다.  
비정형 db인 몽고디비와 소통하기 위해서 데이터 구조를 정의해야 하면 정형 db를 사용하는 것과 차이가 없을 것 같지만 정형 db는 데이터 구조가 변경될 때마다 마이그레이션을 이용하여 릴레이션 구조를 변경해 주어야 하는데 몽고디비는 코드만 변경해주면 되는 차이가 있다.

먼저 프로젝트에 Models/User.cs 경로로 파일을 생성한다.

![2. 구조](../.dummy/101%20몽고/2.%20구조.png)

User.cs 파일은 기존 PostgreSQL에서 만들었던 구조와 똑같이 작성해보겠다.

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

그리고 몽고디비에 맞게 각 부분에 속성을 생성해주겠다.

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

[BsonId]는 데이터의 기본키라는 뜻이고 몽고디비의 기본키의 타입은 ObjectId 타입인데 이를 서버에서 string으로 다루고 싶기에 Id는 string으로 선언해주었고 [BsonRepresentation(BsonType.ObjectId)] 속성을 추가해주었다.  
그리고 [BsonElement] 속성은 파스칼 케이스인 필드 이름을 스네이크 케이스로 몽고디비에 저장되도록 설정하는 속성이다.

## MongoDbSetting.cs

데이터 구조를 만들었으니 몽고디비를 설정하기 위한 클래스를 만들어주겠다.  
프로젝트에 DbSettings/MongoDbSettings.cs 경로로 파일을 만들어준다.

![3. 몽고디비세팅구조](../.dummy/101%20몽고/3.%20몽고디비세팅구조.png)

그리고 MondoDbSettings.cs 파일의 내용은 다음과 같이 작성해준다.

```C#
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string Collection { get; set; } = null!;
}
```

처음에 secrets.json 파일에 추가해준 MongoDbSettings의 키 값과 위 코드의 필드 이름을 동일하게 설정해주어야 한다.
(ConnectionString, Database, Collection 등)

## MongoDbService.cs

그리고 MongoDbSettings을 이용하여 몽고디비를 이용하는 서비스 클래스를 생성해 줄 것이다.  
프로젝트에 Services/MongoDbService.cs 파일을 만들어준다.

![4. 몽고디비서비스](../.dummy/101%20몽고/4.%20몽고디비서비스.png)

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
        // 위와 아래와 같은 코드
        MongoClient client = new MongoClient(options.Value.ConnectionString);
        IMongoDatabase db = client.GetDatabase(options.Value.Database);
        _service = db.GetCollection<User>(options.Value.Collection);
    }
}
```

이렇게 설정하면 MongoDbSetting에서 설정해 둔 Db와 그곳의 컬렉션에서 작업을 해줄 수 있다.

그리고 User가 저장되는 컬렉션에 UserId를 인덱스로 설정해보겠다.  
이는 DB와 소통하는 메서드임으로 비동기적으로 처리하는 것이 권장된다.

이는 MongoDbService 인스턴스 생성 과정에서 설정해주어야 하는데 생성자는 비동기 메서드로 설정이 불가능하므로 따로 만들어주어야 한다.

```C#
public class MongoDbService
{
//..... 생성자
    public static async Task<MongoDbService> CreateAsync(IOptions<MongoDbSettings> options)
    {
        MongoDbService db = new(options);

        // UserId 오름차순 설정
        var index = Builders<User>.IndexKeys.Ascending(u => u.UserId);
        // UserId 고유값 설정
        CreateIndexOptions<User> idxOption = new() { Unique = true };
        // 인덱스 설정
        await db._service.Indexes.CreateOneAsync(new CreateIndexModel<User>(index, idxOption));

        return db;
    }
//..... 다른 메서드들
}
```

이렇게 설정하면 특정 속성의 인덱스를 설정할 수 있으나 웹서비스 빌드 단계에서 ```AddSingleton<MongoDbService>()```으로 간단하게 서비스를 등록할 수 없다.

따라서 직접 인스턴스를 생성하여 등록해주어야 한다.  
Program.cs 파일에 다음과 같은 코드를 추가해준다.
```C#
MongoDbSettings mongoDbSetting = new();

builder.Configuration.GetSection("MongoDbSettings").Bind(mongoDbSetting);

builder.Services.AddSingleton(await MongoDbService.CreateAsync(Options.Create(mongoDbSetting)));
```

## CRUD 구현

그리고 컬렉션에서 작업을 하기 위한 생성, 삭제, 조회 기능을 하는 메서드를 다음과 같이 추가한다.

```C#
public class MongoDbService
{
    private readonly IMongoCollection<User> _service;

    public MongoDbService(IOptions<MongoDbSettings> options)
    {
        _service = new MongoClient(options.Value.ConnectionString)
            .GetDatabase(options.Value.Database)
            .GetCollection<User>(options.Value.Collection);
        // 위와 아래와 같은 코드
        MongoClient client = new MongoClient(options.Value.ConnectionString);
        IMongoDatabase db = client.GetDatabase(options.Value.Database);
        _service = db.GetCollection<User>(options.Value.Collection);
    }
    
    public async Task<List<User>> GetAllAsync() => await _service.Find(_ => true).ToListAsync();

    public async Task<User> GetAsync(string userId) => await _service.Find(u => u.UserId == userId).FirstAsync();

    public async Task PostAsync(User user) => await _service.InsertOneAsync(user);

    public async Task PutAsync(User user) => await _service.ReplaceOneAsync(u => u.UserId == user.UserId, user);

    public async Task DeleteAsync(User user) => await _service.DeleteOneAsync(u => u.Id==user.Id);
}
```

## 테스트

의도한대로 설정이 되었는지 확인하는 코드를 작성해보겠다.

Program.cs 파일에 다음과 같이 구현한다. (브라우저상에서 간단하게 확인해보기 위해 전부 Get 요청으로 구현하였다.)

```C#
var app = builder.Build();

app.MapGet("/get", async (MongoDbService db) =>
{
    List<User> users = await db.GetAllAsync();
    return Results.Ok(users);
});

app.MapGet("/post", async (MongoDbService db) =>
{
    User user = new()
    {
        Username = "홍길동",
        UserId = "honggildong",
        Password = "패스워드",
        Email = "hong@example.com"
    };

    await db.PostAsync(user);

    return Results.Ok("post success");
});

app.MapGet("/put", async (MongoDbService db) =>
{
    User user = await db.GetAsync("honggildong");
    user.Username = "고길동";
    await db.PutAsync(user);

    return Results.Ok("put success");
});

app.MapGet("/del", async (MongoDbService db) =>
{
    User user = await db.GetAsync("honggildong");

    await db.DeleteAsync(user);

    return Results.Ok("delete success");
});

app.Run();
```

![5. Get요청 테스트](../.dummy/101%20몽고/5.%20Get요청%20테스트.png)

Get 요청을 처음 하면 위처럼 빈 데이터가 반환된다.

![6. Post요청 테스트](../.dummy/101%20몽고/6.%20Post요청%20테스트.png)    
![7. Post 이후 Get](../.dummy/101%20몽고/7.%20Post%20이후%20Get.png)

Post 이후 Get 요청을 하면 제대로 데이터가 들어간 것을 확인할 수 있다.

![8. Put 요청 테스트](../.dummy/101 몽고/8. Put 요청 테스트.png)    
![9. Put 이후 Get](../.dummy/101%20몽고/9.%20Put%20이후%20Get.png)

Put 이후 Get 요청을 하면 이름이 변경된 것을 확인할 수 있다.

![10. Delete 요청 테스트](../.dummy/101 몽고/10. Delete 요청 테스트.png)    
![11. Delete 이후 Get](../.dummy/101%20몽고/11.%20Delete%20이후%20Get.png)

마지막으로 Delete 요청 이후 데이터가 사라진 것까지 확인되면 완료된다.

## index 설정 확인

브라우저에 /put 요청을 보내고 ```mongosh -u [유저이름] -p [패스워드] --authenticationDatabase [db이름]``` 으로 접속해본다.

![12. 인덱스 설정 확인](../.dummy/101 몽고/12. 인덱스 설정 확인.png)

user_id 속성이 인덱스로 설정된 것을 확인할 수 있다.

## 마무리
몽고디비를 서버와 연결하고 인덱스 설정 이후 CRUD까지 구현해보았다.
