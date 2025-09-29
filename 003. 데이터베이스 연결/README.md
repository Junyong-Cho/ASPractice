# 데이터베이스 연결

이전 단계에서 PostgreSQL을 설치하고 mydb라는 데이터베이스까지 생성해 주었다.

이제 생성한 데이터베이스와 서버를 연결해 보겠다.

## 라이브러리 추가

연결한 데이터베이스는 SQL문으로 조작하는 게 아닌 C# api로 조작할 것이므로 api를 위한 라이브러리를 추가해 줄 것이다.

구글게 NuGet을 검색하여 NuGet 사이트에 접속한다.

<img width="1205" height="613" alt="1  nuget 사이트" src="https://github.com/user-attachments/assets/f9467d00-2402-456b-8f7c-e2186d21393a" />

검색창에 PostgreSQL을 검색하여 Npgsql.EntityFrameworkCore.PostgreSQL 라이브러리에 들어간다.

<img width="1200" height="719" alt="2  PostgreSQL 검색" src="https://github.com/user-attachments/assets/f35ad4dc-1023-470a-a024-b9f6bf294651" />


.NET CLI에 나오는 명령어를 복사한다.

<img width="876" height="288" alt="3  닷넷 cli 복사" src="https://github.com/user-attachments/assets/58436a4a-8d99-404b-9c56-7d8333b844cc" />

```dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL```  
--version 이후 명령어를 지운 다음(그래야 현재 .net 버전에 맞는 라이브러리가 설치됨) 터미널의 Program.cs 파일이 있는 경로에 명령어를 입력한다.

<img width="1540" height="435" alt="4  npgsql 설치" src="https://github.com/user-attachments/assets/9715a2a8-7880-4495-b112-30904d91619c" />

설치가 완료되면 ```dotnet list package``` 명령어를 입력하여 패키지가 설치되었는지 확인한다.

<img width="924" height="141" alt="5  npgsql 설치 확인" src="https://github.com/user-attachments/assets/13be41a8-1e89-43a5-88db-ef241416f144" />

## DbContext

이제 ASP.NET Core에서 DB를 조작하기 위한 몇 가지 클래스를 만들어야 한다.

처음 api를 구현할 때 만들었던 User.cs 파일 구조를 데이터베이스의 테이블로 만들 것이다.

다만 파일 구조 정리를 위해 User.cs 파일을 프로젝트 밑으로 Models 디렉토리를 만든 다음 그 디렉토리 아래 위치시키도록 하겠다.

파일 구조는 다음과 같다.  

<img width="267" height="227" alt="6  파일 구조" src="https://github.com/user-attachments/assets/1a3ee37b-24a8-4144-ac4d-82393fd15ec7" />

User.cs 파일은 일단 이전에 만들었던 구조 그대로 사용할 것이나 namespace를 추가해 주겠다.  
(record에서 class로 변경해주어야 Patch가 가능한데 후에 릴레이션 구조를 변경하는 방법을 실습해보기 위해 그대로 두겠다.)

```C#
// namespace 프로젝트명.Models 현재 프로젝트는 DbConnection으로 임의로 설정했기에 아래와 같이 명시됨
namespace DbConnection.Models;

public record User(int Id, string Username, string Email);

```

그 다음 데이터베이스를 조작하기 위한 클래스인 DbContext 클래스를 만들어 보겠다.  
다시 프로젝트 밑에 DbContexts 디렉토리를 생성한다.

<img width="276" height="255" alt="7  파일 구조2" src="https://github.com/user-attachments/assets/e29a6287-505a-44e2-9735-45fa4923d30d" />

```C#
using Microsoft.EntityFrameworkCore;

using DbConnection.Models;

namespace DbConnection.DbContexts;

public class UserDbContext : DbContext  // Microsoft.EntityFrameworkCore 라이브러리를 using해 주어야 한다.
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> users { get; set; }
    // 데이터베이스 아래로 users라는 릴레이션이 생성될 것이다.
}
```

Microsoft.EntityFrameworkCore의 DbContext를 상속받는 UserDbContext.cs를 다음과 같이 작성한다.

## ConnectionString

이 다음에 DBMS에 접속하기 위한 설정 코드를 추가해줘야 한다.

솔루션 탐색기를 찾아보면 appsettings.json 파일이 있을 것이다.

<img width="269" height="256" alt="8  앱세팅 제이슨" src="https://github.com/user-attachments/assets/28482933-8184-4173-ba12-9913ef34cb8a" />

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

기본적으로 위와 같이 작성되어 있을 텐데

```json
{
  "ConnectionStrings" : {"Default": "Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=[패스워드]"},
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

이렇게 ConnectionStrings 키를 추가해준다.  
Database에 생성한 데이터베이스 이름을 입력하고 Password에 DBMS 설치 과정에서 설정했던 패스워드를 입력한다. (이 패스워드는 공개되어서는 안 된다.)

그리고 Program.cs 파일에서 WebApplication을 빌드하기 전에 즉 ```var app = builder.Build();``` 이전에 builder 인스턴스에 UserDbContext를 추가해 줄 것이다.

```C#
var builder = WebApplication.CreateBuilder();

string? connectionString = builder.Configuration.GetConnectionString("Default");

//Console.WriteLine(connectionString);
//int t = 1;
//if (t == 1) return;
// 위 코드로 잘 불러왔는지 확인해보기

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();
```

## migrations

이 코드까지 완성되었으면 데이터베이스 구조를 만들어주는 또 다른 라이브러리가 필요해진다.

다시 NuGet 사이트에 접속해서 Design을 검색한다.

<img width="1155" height="416" alt="9  Design 검색" src="https://github.com/user-attachments/assets/a97b8adb-088b-483d-9c8f-1c157a103534" />

아까와 같이 다운로드 명령어를 복사해서 설치한다.

<img width="1318" height="161" alt="10  Design 설치" src="https://github.com/user-attachments/assets/fe2cdbe3-c043-469c-a0a0-57210a3f123e" />

<img width="898" height="218" alt="11  설치 확인" src="https://github.com/user-attachments/assets/d077caed-651c-43de-b35d-6ab532a88ff7" />

그리고 프로젝트 데이터 구조의 변화를 인식하고 데이터베이스 구조를 수정해주는 마이그레이션(migrations)을 만들어 줄 것이다.

우선 마이그레이션 생성을 위한 dotnet 도구를 설치해 주어야 한다.  
```dotnet tool install --global dotnet-ef``` 명령어를 터미널에 입력한다.  
```dotnet ef --version```으로 설치를 확인한다.

<img width="823" height="83" alt="12  닷넷 ef" src="https://github.com/user-attachments/assets/ae1dba44-857f-4b0d-8eae-f1c73724ebe0" />

이제 마이그레이션을 생성하여 데이터베이스에 users라는(UserDbContext에서 선언한 DbSet 이름) 릴레이션을 만들어 보겠다.

```dotnet ef migrations add InicialCreate``` 명령어를 터미널에 입력한다.

<img width="1031" height="98" alt="13  마이그레이션 생성" src="https://github.com/user-attachments/assets/815b2ca2-e9f9-47b1-9177-4540ae811be0" />

생성에 성공하면 솔루션 탐색기에 migrations라는 디렉토리가 추가된 것을 확인할 수 있다.

<img width="279" height="273" alt="14  마이그레이션" src="https://github.com/user-attachments/assets/490bd53e-09bf-45d2-bb98-d5fb1f08b7ef" />

migrations 디렉터리 밑으로 IniticalCreate.cs 파일이 생성되는데 db 구조가 변경될 때마다(```dotnet ef migrations add (변경 내용)``` 명령어를 실행할 때마다) 깃허브 버전 관리처럼 변경 이력을 확인할 수 있다.

그리고 ```dotnet ef database update``` 명령어를 실행하여 users 릴레이션을 생성한다.

다음 그림과 같이 실행되면 설공한 것이다.

<img width="1193" height="578" alt="15  데이터베이스 업데이트" src="https://github.com/user-attachments/assets/6b018e71-62fc-40ef-8a48-b5eb2bbe866f" />

처음에 fail이 나온 것은 마이그레이션이 데이터베이스를 조작하기 전에 마이그레이션의 이력을 db에서 찾는데 처음 db를 업데이트하면 __EFMigrationsHistory라는 마이그레이션의 이력이 저장된 릴레이션이 존재하지 않기 때문에 __EFMigrationsHistory을 찾을 수 없다는 실패 메세지이며 자동으로 __EFMigrationsHistory가 생성된다.

그러면 db에 실제로 릴레이션이 생성되었는지 확인해보겠다.

```psql -U postgres```라는 명령어를 터미널에 입력하여 패스워드와 함께 DBMS에 접속한다.

```\c [생성한 db 이름]```으로 db를 이동한다.

```\dt```로 현재 db에 존재하는 릴레이션을 조회한다.

<img width="805" height="384" alt="16  생성 확인" src="https://github.com/user-attachments/assets/fbde3a84-458e-4e97-8a28-9d013dcbc76c" />

그림처럼 UserDbContext에서 선언된 DbSet 이름인 users와 __EFMigrationsHistory 릴레이션이 생성된 것을 확인할 수 있을 것이다.



## 주의점
connectionString의 Password를 appsettings.json 파일에 그대로 집어넣으면 패스워드를 깃허브 같은 곳에 실수로 공개해버리는 일이 생길 수 있다.  
따라서 connectionString은 프로젝트에 저장하기보다 개발 단계에서는 로컬 secret 코드로 안전하게 보관했다가 배포 이후 실제 서버에서 appsettings.json 파일에 추가해 주는 것이 안전하다.  
로컬 시크릿 코드에 저장하는 방법은 다음과 같다.
  
<img width="356" height="444" alt="17  사용자 암호 관리" src="https://github.com/user-attachments/assets/dcbd3ab3-2b39-4e96-ae24-53876e8a7ac5" />

솔루션 탐색기의 프로젝트를 우클릭 후 사용자 암호 관리를 선택하면 secrets.json 파일이 생성될 것이다.

<img width="414" height="405" alt="18  secret json 경로" src="https://github.com/user-attachments/assets/4d3a19a8-717b-44f6-9b98-18aa1b72850b" />

생성된 secrets.json 파일의 상위폴더를 들어가 보면 ```C:\Users\유저 이름\AppData\Roaming\Microsoft\UserSecrets``` 경로로 디렉터리가 생긴 것을 확인할 수 있다.
생성된 디렉터리 밑으로 secrets.json 파일이 생성되어 있을 것이다.

그 다음 visual studio를 재부팅하면 프로젝트에 secrets.json 파일에 secrets.json가 속한 디렉터리 이름이 추가될 것인데 이를 확인하는 방법은 솔루션 탐색기에 보이지 않는 .csproj 파일을 확인해 보면 알 수 있다.

마지막으로 secrets.json 파일에 실제 패스워드를 담은 다음과 같은 스크립트를 추가하면 된다.

```json
"ConnectionStrings" : { "Default": "Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=[패스워드]"}
```

이렇게 secrets.json 파일을 만들면 개발 환경으로 프로젝트를 실행했을 때 이 파일의 ConnectionString을 불러올 수 있는데 한 가지 문제가 있다.  
```dotnet run```으로 실행했을 때는 기본적으로 개발 환경으로 실행되도록 설정되어 있어 정상적으로 읽어올 수 있지만 ```dotnet ef```로 마이그레이션을 생성하게 되면 secrets.json 파일을 못 읽는 것이다.  
따라서 ```dotnet ef``` 명령어를 실행했을 때에도 정상적으로 ConnectionString을 불러오기 위한 코드를 추가해 주어야 한다.  

```C#
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>().Build();
```

위 코드로 config에 sercrets.json을 담고

```C#
string? connectionString = config["ConnectionStrings:Default"];
connectionString ??= builder.Configuration.GetConnectionString("Default");
// connectionString이 null이면 ??= 오른쪽 값 대입
```

아니면 다음과 같이 builder.Configuration에 직접 UserSecret을 추가해주어도 된다.

```C#
builder.Configuration.AddUserSecrets<Program>();

string connectionString = builder.Configuration.GetConnectionString("Default");
```

이렇게 secrets.json 파일에 저장된 ConnectionString을 읽어올 수 있다.

# 마무리

이것으로 Db와 프로젝트를 연결해 보았다.
