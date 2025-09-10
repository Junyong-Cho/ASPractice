# �����ͺ��̽� ����

���� �ܰ迡�� PostgreSQL�� ��ġ�ϰ� mydb��� �����ͺ��̽����� ������ �־���.

���� ������ �����ͺ��̽��� ������ ������ ���ڴ�.

## ���̺귯�� �߰�

������ �����ͺ��̽��� SQL������ �����ϴ� �� �ƴ� C# api�� ������ ���̹Ƿ� api�� ���� ���̺귯���� �߰��� �� ���̴�.

���۰� NuGet�� �˻��Ͽ� NuGet ����Ʈ�� �����Ѵ�.

![1. Nuget ����Ʈ](../dummy/4%20DB%20����/1.%20nuget%20����Ʈ.png)

�˻�â�� PostgreSQL�� �˻��Ͽ� Npgsql.EntityFrameworkCore.PostgreSQL ���̺귯���� ����.
![2. Postgre S Q L �˻�](../dummy/4%20DB%20����/2.%20PostgreSQL%20�˻�.png)

.NET CLI�� ������ ��ɾ �����Ѵ�.

![3. ��� Cli ����](../dummy/4%20DB%20����/3.%20���%20cli%20����.png)

```dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL```  
--version ���� ��ɾ ���� ����(�׷��� ���� .net ������ �´� ���̺귯���� ��ġ��) �͹̳��� Program.cs ������ �ִ� ��ο� ��ɾ �Է��Ѵ�.

![4. Npgsql ��ġ](../dummy/4%20DB%20����/4.%20npgsql%20��ġ.png)

��ġ�� �Ϸ�Ǹ� ```dotnet list package``` ��ɾ �Է��Ͽ� ��Ű���� ��ġ�Ǿ����� Ȯ���Ѵ�.

![5. Npgsql ��ġ Ȯ��](../dummy/4%20DB%20����/5.%20npgsql%20��ġ%20Ȯ��.png)

## DbContext

���� ASP.NET Core���� DB�� �����ϱ� ���� �� ���� Ŭ������ ������ �Ѵ�.

ó�� api�� ������ �� ������� User.cs ���� ������ �����ͺ��̽��� ���̺�� ���� ���̴�.

�ٸ� ���� ���� ������ ���� User.cs ������ ������Ʈ ������ Models ���丮�� ���� ���� �� ���丮 �Ʒ� ��ġ��Ű���� �ϰڴ�.

���� ������ ������ ����.  
![6. ���� ����](../dummy/4%20DB%20����/6.%20����%20����.png)

User.cs ������ �ϴ� ������ ������� ���� �״�� ����� ���̳� namespace�� �߰��� �ְڴ�.  
(record���� class�� �������־�� Patch�� �����ѵ� �Ŀ� �����̼� ������ �����ϴ� ����� �ǽ��غ��� ���� �״�� �ΰڴ�.)

```C#
// namespace ������Ʈ��.Models ���� ������Ʈ�� DbConnection���� ���Ƿ� �����߱⿡ �Ʒ��� ���� ��õ�
namespace DbConnection.Models;

public record User(int Id, string Username, string Email);

```

�� ���� �����ͺ��̽��� �����ϱ� ���� Ŭ������ DbContext Ŭ������ ����� ���ڴ�.  
�ٽ� ������Ʈ �ؿ� DbContexts ���丮�� �����Ѵ�.

![7. ���� ����2](../dummy/4%20DB%20����/7.%20����%20����2.png)

```C#
using Microsoft.EntityFrameworkCore;

using DbConnection.Models;

namespace DbConnection.DbContexts;

public class UserDbContext : DbContext  // Microsoft.EntityFrameworkCore ���̺귯���� using�� �־�� �Ѵ�.
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> users { get; set; }
    // �����ͺ��̽� �Ʒ��� users��� �����̼��� ������ ���̴�.
}
```

Microsoft.EntityFrameworkCore�� DbContext�� ��ӹ޴� UserDbContext.cs�� ������ ���� �ۼ��Ѵ�.

## ConnectionString

�� ������ DBMS�� �����ϱ� ���� ���� �ڵ带 �߰������ �Ѵ�.

�ַ�� Ž���⸦ ã�ƺ��� appsettings.json ������ ���� ���̴�.

![8. �ۼ��� ���̽�](../dummy/4%20DB%20����/8.%20�ۼ���%20���̽�.png)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

�⺻������ ���� ���� �ۼ��Ǿ� ���� �ٵ�

```json
{
  "ConnectionStrings" : {"Default": "Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=[�н�����]"},
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

�̷��� ConnectionStrings Ű�� �߰����ش�.  
Database�� ������ �����ͺ��̽� �̸��� �Է��ϰ� Password�� DBMS ��ġ �������� �����ߴ� �н����带 �Է��Ѵ�. (�� �н������ �����Ǿ�� �� �ȴ�.)

�׸��� Program.cs ���Ͽ��� WebApplication�� �����ϱ� ���� �� ```var app = builder.Build();``` ������ builder �ν��Ͻ��� UserDbContext�� �߰��� �� ���̴�.

```C#
var builder = WebApplication.CreateBuilder();

string? connectionString = builder.Configuration.GetConnectionString("Default");

//Console.WriteLine(connectionString);
//int t = 1;
//if (t == 1) return;
// �� �ڵ�� �� �ҷ��Դ��� Ȯ���غ���

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();
```

## migrations

�� �ڵ���� �ϼ��Ǿ����� �����ͺ��̽� ������ ������ִ� �� �ٸ� ���̺귯���� �ʿ�������.

�ٽ� NuGet ����Ʈ�� �����ؼ� Design�� �˻��Ѵ�.

![9. Design �˻�](../dummy/4%20DB%20����/9.%20Design%20�˻�.png)

�Ʊ�� ���� �ٿ�ε� ��ɾ �����ؼ� ��ġ�Ѵ�.

![10. Design ��ġ](../dummy/4%20DB%20����/10.%20Design%20��ġ.png)

![11. ��ġ Ȯ��](../dummy/4%20DB%20����/11.%20��ġ%20Ȯ��.png)

�׸��� ������Ʈ ������ ������ ��ȭ�� �ν��ϰ� �����ͺ��̽� ������ �������ִ� ���̱׷��̼�(migrations)�� ����� �� ���̴�.

�켱 ���̱׷��̼� ������ ���� dotnet ������ ��ġ�� �־�� �Ѵ�.  
```dotnet tool install --global dotnet-ef``` ��ɾ �͹̳ο� �Է��Ѵ�.  
```dotnet ef --version```���� ��ġ�� Ȯ���Ѵ�.

![12. ��� Ef](../dummy/4%20DB%20����/12.%20���%20ef.png)

���� ���̱׷��̼��� �����Ͽ� �����ͺ��̽��� users���(UserDbContext���� ������ DbSet �̸�) �����̼��� ����� ���ڴ�.

```dotnet ef migrations add InicialCreate``` ��ɾ �͹̳ο� �Է��Ѵ�.

![13. ���̱׷��̼� ����](../dummy/4%20DB%20����/13.%20���̱׷��̼�%20����.png)

������ �����ϸ� �ַ�� Ž���⿡ migrations��� ���丮�� �߰��� ���� Ȯ���� �� �ִ�.

![14. ���̱׷��̼�](../dummy/4%20DB%20����/14.%20���̱׷��̼�.png)

migrations ���͸� ������ IniticalCreate.cs ������ �����Ǵµ� db ������ ����� ������(```dotnet ef migrations add (���� ����)``` ��ɾ ������ ������) ����� ���� ����ó�� ���� �̷��� Ȯ���� �� �ִ�.

�׸��� ```dotnet ef database update``` ��ɾ �����Ͽ� users �����̼��� �����Ѵ�.

���� �׸��� ���� ����Ǹ� ������ ���̴�.

![15. �����ͺ��̽� ������Ʈ](../dummy/4%20DB%20����/15.%20�����ͺ��̽�%20������Ʈ.png)

ó���� fail�� ���� ���� ���̱׷��̼��� �����ͺ��̽��� �����ϱ� ���� ���̱׷��̼��� �̷��� db���� ã�µ� ó�� db�� ������Ʈ�ϸ� __EFMigrationsHistory��� ���̱׷��̼��� �̷��� ����� �����̼��� �������� �ʱ� ������ __EFMigrationsHistory�� ã�� �� ���ٴ� ���� �޼����̸� �ڵ����� __EFMigrationsHistory�� �����ȴ�.

�׷��� db�� ������ �����̼��� �����Ǿ����� Ȯ���غ��ڴ�.

```psql -U postgres```��� ��ɾ �͹̳ο� �Է��Ͽ� �н������ �Բ� DBMS�� �����Ѵ�.

```\c [������ db �̸�]```���� db�� �̵��Ѵ�.

```\dt```�� ���� db�� �����ϴ� �����̼��� ��ȸ�Ѵ�.

![16. ���� Ȯ��](../dummy/4%20DB%20����/16.%20����%20Ȯ��.png)

�׸�ó�� UserDbContext���� ����� DbSet �̸��� users�� __EFMigrationsHistory �����̼��� ������ ���� Ȯ���� �� ���� ���̴�.



## ������
connectionString�� Password�� appsettings.json ���Ͽ� �״�� ��������� �н����带 ����� ���� ���� �Ǽ��� �����ع����� ���� ���� �� �ִ�.  
���� connectionString�� ������Ʈ�� �����ϱ⺸�� ���� �ܰ迡���� ���� secret �ڵ�� �����ϰ� �����ߴٰ� ���� ���� ���� �������� appsetting.json ���Ͽ� �߰��� �ִ� ���� �����ϴ�.  
���� ��ũ�� �ڵ忡 �����ϴ� ����� ������ ����.
  
![101. ����� ��ȣ ����](../dummy/4 DB ����/101. ����� ��ȣ ����.png)

�ַ�� Ž������ ������Ʈ�� ��Ŭ�� �� ����� ��ȣ ������ �����ϸ� secrets.json ������ ������ ���̴�.

![102. Secret Json ���](../dummy/4 DB ����/secret json ���.png)

������ secrets.json ������ ���������� �� ���� ```C:\Users\���� �̸�\AppData\Roaming\Microsoft\UserSecrets``` ��η� ���͸��� ���� ���� Ȯ���� �� �ִ�.
������ ���͸� ������ secrets.json ������ �����Ǿ� ���� ���̴�.

�� ���� visual studio�� ������ϸ� ������Ʈ�� secrets.json ���Ͽ� secrets.json�� ���� ���͸� �̸��� �߰��� ���ε� �̸� Ȯ���ϴ� ����� �ַ�� Ž���⿡ ������ �ʴ� .csproj ������ Ȯ���� ���� �� �� �ִ�.

���������� secrets.json ���Ͽ� ���� �н����带 ���� ������ ���� ��ũ��Ʈ�� �߰��ϸ� �ȴ�.

```json
"ConnectionStrings" : { "Default": "Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=[�н�����]"}
```

�̷��� secrets.json ������ ����� ���� ȯ������ ������Ʈ�� �������� �� �� ������ ConnectionString�� �ҷ��� �� �ִµ� �� ���� ������ �ִ�.  
```dotnet run```���� �������� ���� �⺻������ ���� ȯ������ ����ǵ��� �����Ǿ� �־� ���������� �о�� �� ������ ```dotnet ef```�� ���̱׷��̼��� �����ϰ� �Ǹ� secrets.json ������ �� �д� ���̴�.  
���� ```dotnet ef``` ��ɾ �������� ������ ���������� ConnectionString�� �ҷ����� ���� �ڵ带 �߰��� �־�� �Ѵ�.  

```C#
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>().Build();
```

�� �ڵ�� config�� sercrets.json�� ���

```C#
string? connectionString = config["ConnectionStrings:Default"];
connectionString ??= builder.Configuration.GetConnectionString("Default");
// connectionString�� null�̸� ??= ������ �� ����
```

�̷��� secrets.json ���Ͽ� ����� ConnectionString�� �о�� �� �ִ�.

# ������

�̰����� Db�� ������Ʈ�� ������ ���Ҵ�.