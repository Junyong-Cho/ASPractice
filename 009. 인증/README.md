# 인증

우리는 지금까지 User 생성 요청이 들어오면 등록해주고 id에 따라 조회해주는 기능까지만 구현했다.  

이제 등록한 유저의 로그인 기능을 만들어보겠다.

어떠한 유저가 Id와 패스워드를 입력하면 db에서 해당 ID의 유저가 존재하는지 확인하고 존재하면 패스워드가 같은지 확인한 다음에 로그인 성공 여부를 반환하도록 해보겠다.

## SignupUserDto, SigninUserDto 생성

먼저 인증 데이터를 주고받을 Dto를 만들어주겠다. SignupUserDto는 CreateUser와 동작은 비슷하지만 회원가입은 인증을 관리하는 기능이므로 Create보단 Sign up으로 명시하는 것이 더 좋을 것 같아서 CreateUserDto 데이터 형식을 제거한 다음에 새로운 SignupUserDto를 만들어보겠다.

인증을 위한 Dto들을 따로 관리하도록 Dtos 디렉토리 밑으로 AuthDtos 디렉토리를 새로 만든 다음에 SigninUserDto.cs와 SingupUserDto.cs 파일을 만든다.

<img width="320" height="222" alt="1  Auth 디렉터리" src="https://github.com/user-attachments/assets/b7306f98-c574-445f-b2b6-64ff3123c708" />

그 다음 SignupUserDto 클래스를 다음과 같이 작성한다.  
이전에 작성했던 CreateUserDto와 같다.  

```C#
namespace Authentication.Dtos.AuthDtos;

public class SignupUserDto
{
    public string UserId { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
```

그리고 로그인할 때 사용되는 SigninUserDto는 다음과 같이 작성한다.  
로그인에는 UserId와 패스워드만 사용될 것이다.

```C#
namespace Authentication.Dtos.AuthDtos;

public class SigninUserDto
{
    public string UserId { get; set; } = null!;
    public string Password { get; set; } = null!;
}
```

## UserId 인덱스 설정

SigninUserDto에 포함된 데이터는 유저 id와 패스워드 뿐이다.  
지금까지 db에서 엔티티를 찾을 때 엔티티의 기본키로 조회해서 찾았는데 기본키는 db에서 자동으로 생성하는 번호이므로 로그인할 때는 UserId로 조회하도록 할 것이다.  

유저 id로 엔티티를 조회하는 방법은 다음과 같다.

```C#
app.MapGet("/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.Users.FindAsync(id);
    /*기본키로 조회*/
});

app.MapGet("/{userId}", async (UserDbContext db, string userId) =>
{
    User? user = await db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

    /*기본키가 아닌 속성으로 조회*/
});
```

그러나 기본키가 아닌 속성으로 조회하게 되면 릴레이션의 모든 UserId 값을 조회해서 매칭되는 값이 있는지 확인해봐야 하므로 만약 존재하지 않는 UserId라면(즉 최악의 경우라면) 엔티티 탐색의 시간복잡도가 O(n)이 된다.

따라서 효율적으로 UserId를 조회하기 위해 User 릴레이션의 UserId 속성을 인덱스로 설정해서 조회에 최악의 경우에도 O(log n)이 걸리도록 설정하겠다.

### UserDbContext 오버라이드

UserDbContext.cs 파일을 다음과 같이 수정했다.
```C#
public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    //오버라이드 추가
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users")    // 릴레이션 이름 설정
            .HasIndex(u => u.UserId).IsUnique();        // UserId 인덱스 및 Unique 설정
    }
}
```

인덱스를 설정하는 김에 릴레이션 이름을 users로 설정해줬다.  
UserDbContext의 DbSet의 이름을 C# 명명 약속에 맞게 Users로 설정해주었기 때문에 이 설정이 없으면 db에 Users라는 이름의 릴레이션이 생성되는데 이는 SQL문으로 조회할 때 select * from "Users"라고 따옴표 표시를 해 주어야 하는 귀찮음이 있기에 소문자로 설정해준것이다.

그리고 HasIndex()로 UserId를 인덱스로 설정해준다.  
이렇게 UserId 속성이 db에서 정렬되면서 이진탐색으로 O(log n)의 탐색이 가능해진다.  
더하여 UserId는 중복되면 안 되므로 IsUnique()로 중복 불가 설정도 해주었다.

db 구조를 변경하였으므로 ```dotnet ef migrations add (변경 내용)``` ```dotnet ef database update``` 명령어로 db를 업데이트해준다.

## 패스워드 해싱

일반적으로 유저 데이터를 저장할 때 패스워드는 해싱해서 저장한다.  
이번 단계에서는 그렇게 해 볼 것이다.

먼저 nuget 사이트에서 BCrypt를 검색해서 BCrypt.Net-Next를 설치한다.

<img width="1188" height="429" alt="2  BCrypt Net Next" src="https://github.com/user-attachments/assets/463a62db-9c99-4f9c-8ee4-9b61e289433c" />

그리고 signup 요청이 들어오면 동작할 메서드를 다음과 같이 정의해준다.

```C#
app.MapPost("/signup", async (UserDbContext db, SignupUserDto signupUser, IValidator<SignupUserDto> validator, IMapper mapper) =>
{
    var results = validator.Validate(signupUser);       // 유효성 검사

    if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

    if (await db.Users.AnyAsync(u => u.UserId == signupUser.UserId)) return Results.Conflict();     // id 중복 검사

    User user = mapper.Map<User>(signupUser);

    user.HashPassword = BCrypt.Net.BCrypt.HashPassword(signupUser.Password);        // 패스워드 해싱

    db.Users.Add(user);

    await db.SaveChangesAsync();

    return Results.NoContent();
});
```

## JWT(Json Web Token)

로그인에 성공한 유저는 원하는 서비스를 요청할 수 있다.  
그리고 서버는 유저가 서비스를 요청할 때 접근 권한이 있는지(로그인에 성공한 유저인지) 확인해야 한다.  
서버가 유저의 권한을 확인하는 방법은 여러 가지가 있는데 여기서는 토큰을 사용하여 권한을 확인하도록 해보겠다.

토큰의 원리는 간단히 서버가 유저에게 부여하는 확인증 같은 거라고 할 수 있다.  
로그인에 성공한 유저에게 서버가 토큰을 보내주고 유저가 어떤 서비스를 이용하고자 할 때 토큰과 함께 요청을 보내면 서버는 그 토큰이 자신이 부여한 것이 맞는지 그리고 유효한 토큰인지 확인한 다음에 서비스를 제공하게 된다.

먼저 nuget 사이트에 접속해서 Microsoft.AspNetCore.Authentication.JwtBearer를 설치한다.

<img width="1251" height="799" alt="3  JwtBearer" src="https://github.com/user-attachments/assets/4c98102c-4569-41ec-a739-38f30a4d5ec3" />

그리고 유저 인증을 위한 대칭 키를 생성해주겠다.  
openssl을 이용하여 생성해 주어도 되고 C# 코드로 간단하게 생성해주어도 된다.  
윈도우 기준 openssl은 설치를 해야 하므로 C# 코드로 128바이트 길이로 생성해 보겠다.

```C#
using System.Security.Cryptography;
using static System.Console;

byte[] key = new byte[128];

RandomNumberGenerator.Fill(key);

Write(Convert.ToBase64String(key));
```

다음 코드를 실행하면 랜덤으로 생성된 키를 base64로 인코딩한 결과를 얻을 수 있다.  
이 키를 secrets.json 파일에 다음과 같은 양식으로 추가한다.

```json
"Jwt" : {"Key" :"G+bRj/IJg5K9/lvnrnyQEBxo1sJ3jbdU67ojyNggUxtjuqR202WzmByA8qxoZWj6CocDM1QU5Io0WJHi7IekCyVCQCuSLK/gBJ/cmGK1VfxZxtTOlPUN49mzuwNzbffekon/hk7MZhmRC2yFeZaAHtynvLXVg4z3y2brqwYx7D4="}
            // 랜덤으로 생성한 키
```

이제 로그인 요청이 들어오면 인증 후 토큰을 발급하는 코드를 작성해보겠다.

```C#
app.MapPost("/signin", async (HttpContext context, UserDbContext db, IValidator<SigninUserDto> validator, IConfiguration config, SigninUserDto signinUser) =>
{
    var results = validator.Validate(signinUser);

    if (!results.IsValid) return Results.Unauthorized();

    User? user = await db.Users.FirstOrDefaultAsync(u => u.UserId==signinUser.UserId);

    if (user == null) return Results.Unauthorized();

    if (!BCrypt.Net.BCrypt.Verify(signinUser.Password, user.HashPassword)) return Results.Unauthorized();

    SigningCredentials credential = new(new SymmetricSecurityKey(Convert.FromBase64String(config["Jwt:Key"]!)),
        SecurityAlgorithms.HmacSha256);

    Claim[] claims_ =
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Name, user.Username),
        new Claim(JwtRegisteredClaimNames.Email, user.Email)
    };

    JwtSecurityToken token = new(               // 토큰 생성
        issuer: config["Issuer"],               // 토큰 발급자 (현재 null)
        audience: config["Audience"],           // 토큰을 받는 클라이언트 (현재 null)
        claims: claims_,                        // 사용자 정보
        signingCredentials: credential,         // 인증서
        expires: DateTime.Now.AddMinutes(60));  // 토큰 유효시간 60분

    string stringToken = new JwtSecurityTokenHandler().WriteToken(token);

    CookieOptions cookieOptions = new()
    {
        HttpOnly = true,                        // 웹브라우저에서 자동으로 쿠키 처리하기
        Secure = true,                          // https 프로토콜만 허용
        Expires = DateTime.Now.AddMinutes(60)   // 토큰 유효 기간 60분
    };

    context.Response.Cookies.Append("jwt-token", stringToken, cookieOptions);
    // HttpContext 쿠키에 토큰 추가

    return Results.Ok(new { Message = "SUCCESS!" });
});
```
로그인을 시도했을 때 어디가 문제인지 친절히 알려주는 것은 보안상 문제가 있을 거라고 생각해 유효성 문제가 발생했을 때에도 Unathorized를 반환하도록 하였다.

그리고 토큰을 전송할 때 클라이언트측에서 토큰을 처리하기보다 웹브라우저가 자동으로 처리하도록(즉 웹페이지의 javascript가 처리하지 않도록) 쿠키 옵션을 설정하고 HttpContext에 저장하여 전송하도록 작성였다.

또 쿠키 옵션에 ```Secure = true``` 부분은 https 프로토콜만 처리하도록 하는 옵션이므로 appsettings.json 파일에 다음 코드를 추가한다.

```json
"Urls" : "https://localhost:5009" // 포트 번호 자유
```

마지막으로 로그아웃 코드는 다음과 같다.

```C#
app.MapPost("/signout", (HttpContext context) =>
{
    context.Response.Cookies.Delete("jwt-token");
    return Results.Ok(new { Message = "SUCCESS" });
}).RequireAuthorization();
```

## 토큰 인증 요청 처리

이제 서버에서 토큰을 확인하도록 설정을 해주겠다.

```C#
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,    // 인증서 검증
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:Key"]!)), // 인증서 키
            ValidateIssuer = false,             // Issuer 검증 안함
            ValidateAudience = false            // Audience 검증 안함
        };

        options.Events = new()                  // 웹브라우저가 자동으로 처리한 토큰을 지정
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("jwt-token"))       // 서버에서 보낸 토큰이 존재하면
                    context.Token = context.Request.Cookies["jwt-token"];   // 서버가 확인해야 하는 토큰 지정
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
```

이제 토큰이 존재하는 브라우저에서 요청이 들어올 경우에만 요청을 처리해주는 엔드포인트를 간단하게 만들어보겠다.

```C#
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/hello", () => "Hello user").RequireAuthorization();
```

위와 같이 빌더에서 설정해 준 Authentication과 Authorization을 추가해주고 요청 매핑 설정 메서드 뒤에 RequireAuthorization() 메서드를 추가해주면 요청을 처리하기 전에 인증 여부먼저 확인할 것이다.

그리고 회원가입과 로그인 폼을 gemini에게 작성시켰다.  
로그인에 필요한 데이터는 UserId와 Password이고 /auth/signin으로 보내도록 하였고  
회원가입에 필요한 데이터는 UserId, Password, Email, Username이고 /auth/signup으로 보내도록 작성하라 했다.  
마지막으로 로그인에 성공한 유저가 클릭하면 환영합니다를 출력하도록 하는 버튼과 로그아웃 버튼도 추가했다.

gemini가 작성해준 코드들은 다음과 같다.

index.html
```html
<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>로그인 / 회원가입</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>

    <div class="container">
        <!-- 회원가입 폼 -->
        <div id="signup-section">
            <h2>회원가입</h2>
            <form id="signup-form">
                <div class="form-group">
                    <label for="reg-userid">사용자 ID</label>
                    <input type="text" id="reg-userid" required>
                </div>
                <div class="form-group">
                    <label for="reg-username">이름</label>
                    <input type="text" id="reg-username" required>
                </div>
                <div class="form-group">
                    <label for="reg-email">이메일</label>
                    <input type="email" id="reg-email" required>
                </div>
                <div class="form-group">
                    <label for="reg-password">비밀번호</label>
                    <input type="password" id="reg-password" required>
                </div>
                <button type="submit">가입하기</button>
            </form>
        </div>

        <hr>

        <!-- 로그인 폼 -->
        <div id="signin-section">
            <h2>로그인</h2>
            <form id="signin-form">
                <div class="form-group">
                    <label for="login-userid">사용자 ID</label>
                    <input type="text" id="login-userid" required>
                </div>
                <div class="form-group">
                    <label for="login-password">비밀번호</label>
                    <input type="password" id="login-password" required>
                </div>
                <button type="submit">로그인</button>
            </form>
        </div>

        <hr>

        <!-- 인증 테스트 섹션 -->
        <div id="test-section">
            <h2>인증 테스트</h2>
            <button id="myname-button">내 이름 확인하기</button>
            <button id="signout-button" class="secondary">로그아웃</button>
        </div>

        <div id="message"></div>
    </div>

    <script src="js/app.js"></script>
</body>
</html>
```

style.css
```css
body {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background-color: #f0f2f5;
    margin: 0;
    padding: 20px 0;
}

.container {
    background: white;
    padding: 2rem;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    width: 100%;
    max-width: 400px;
}

h2 {
    text-align: center;
    color: #333;
    margin-top: 0;
}

hr {
    border: none;
    border-top: 1px solid #eee;
    margin: 2rem 0;
}

.form-group {
    margin-bottom: 1rem;
}

label {
    display: block;
    margin-bottom: 0.5rem;
    color: #555;
    font-weight: 600;
}

input {
    width: 100%;
    padding: 0.75rem;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-sizing: border-box;
    font-size: 1rem;
}

button {
    width: 100%;
    padding: 0.75rem;
    border: none;
    border-radius: 4px;
    background-color: #007bff;
    color: white;
    font-size: 1rem;
    font-weight: bold;
    cursor: pointer;
    transition: background-color 0.2s;
}

    button:hover {
        background-color: #0056b3;
    }

#test-section button {
    background-color: #28a745;
    margin-top: 0.5rem;
}

    #test-section button:hover {
        background-color: #218838;
    }

button.secondary {
    background-color: #6c757d;
}

    button.secondary:hover {
        background-color: #5a6268;
    }

#message {
    margin-top: 1rem;
    text-align: center;
    font-weight: bold;
    min-height: 1.2em;
}

.success {
    color: #28a745;
}

.error {
    color: #dc3545;
}
```

app.js

```js
document.addEventListener('DOMContentLoaded', () => {
    const signupForm = document.getElementById('signup-form');
    const signinForm = document.getElementById('signin-form');
    const myNameButton = document.getElementById('myname-button');
    const signoutButton = document.getElementById('signout-button');
    const messageDiv = document.getElementById('message');

    // 메시지 표시 함수
    const showMessage = (text, type) => {
        messageDiv.textContent = text;
        messageDiv.className = type; // 'success' 또는 'error'
    };

    // 회원가입 폼 제출 이벤트
    if (signupForm) {
        signupForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const userId = document.getElementById('reg-userid').value;
            const username = document.getElementById('reg-username').value;
            const email = document.getElementById('reg-email').value;
            const password = document.getElementById('reg-password').value;

            try {
                const response = await fetch('/auth/signup', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ userId, username, email, password })
                });

                if (response.ok) {
                    showMessage('회원가입 성공!', 'success');
                    signupForm.reset();
                } else {
                    const errorData = await response.json();
                    const firstError = errorData.errors ? Object.values(errorData.errors)[0][0] : (errorData.message || response.statusText);
                    showMessage(`회원가입 실패: ${firstError}`, 'error');
                }
            } catch (error) {
                showMessage('네트워크 오류가 발생했습니다.', 'error');
            }
        });
    }

    // 로그인 폼 제출 이벤트
    if (signinForm) {
        signinForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const userId = document.getElementById('login-userid').value;
            const password = document.getElementById('login-password').value;

            try {
                const response = await fetch('/auth/signin', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ userId, password })
                });

                if (response.ok) {
                    showMessage('로그인 성공! 이제 버튼을 테스트해보세요.', 'success');
                } else {
                    showMessage('로그인 실패: ID 또는 비밀번호를 확인하세요.', 'error');
                }
            } catch (error) {
                showMessage('네트워크 오류가 발생했습니다.', 'error');
            }
        });
    }

    // 인증 테스트 버튼 클릭 이벤트
    if (myNameButton) {
        myNameButton.addEventListener('click', async () => {
            try {
                const response = await fetch('/myname', {
                    method: 'GET'
                });

                if (response.ok) {
                    const data = await response.text();
                    showMessage(data, 'success');
                } else {
                    showMessage(`인증 실패: ${response.status} (로그인이 필요합니다)`, 'error');
                }
            } catch (error) {
                showMessage('네트워크 오류가 발생했습니다.', 'error');
            }
        });
    }

    // 로그아웃 버튼 클릭 이벤트
    if (signoutButton) {
        signoutButton.addEventListener('click', async () => {
            try {
                const response = await fetch('/auth/signout', {
                    method: 'POST'
                });

                if (response.ok) {
                    showMessage('로그아웃 되었습니다.', 'success');
                } else {
                    showMessage('로그아웃 실패. (로그인 상태가 아닐 수 있습니다)', 'error');
                }
            } catch (error) {
                showMessage('네트워크 오류가 발생했습니다.', 'error');
            }
        });
    }
});
```

각각 wwwroot, wwwroot/css, wwwroot/js에 집어넣고 다음 코드를 통해 전송하도록 했다.

```C#
app.UseStaticFiles();

app.MapGet("/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/index.html");
});
```

마지막으로 로그인에 성공한 유저가 버튼을 누르면 동작하는 메서드를 작성했다.

```C#
app.MapGet("/myname", async (HttpContext context, UserDbContext db) =>
{
    string? id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    User? user = await db.Users.FindAsync(int.Parse(id!));

    return $"환영합니다. {user.Username}님";
}).RequireAuthorization();
```

## 테스트

이제 프로젝트를 실행하고 ```https://localhost:5009```로 접속한다. (https를 명시해 주어야 한다.)

<img width="343" height="842" alt="4  메인페이지" src="https://github.com/user-attachments/assets/05f659a7-f43a-4908-ba01-038457df5b96" />

로그인을 안 하면 다음처럼 인증에 실패한다.  

<img width="320" height="170" alt="5  이름 실패" src="https://github.com/user-attachments/assets/db6c3aba-7a77-411a-9d62-223559165317" />  
<img width="332" height="176" alt="6  로그아웃 실패" src="https://github.com/user-attachments/assets/b686f54a-6994-4cc5-b6da-71c46246d919" />  


양식에 맞게 정보를 입력하면 회원가입에 성공한다.

<img width="316" height="841" alt="7  회원가입 성공" src="https://github.com/user-attachments/assets/04e7206c-7a6e-4287-bead-56cd1f24b22e" />

실제 db에 접속해보면 패스워드 해싱까지 잘 된 것을 확인할 수 있다.

<img width="1009" height="107" alt="8  가입확인" src="https://github.com/user-attachments/assets/7b6f7f35-3d2a-4e08-96d4-dfc1032060e9" />

회원가입한 정보대로 로그인하면 로그인에 성공한다.

<img width="329" height="437" alt="9  로그인 성공" src="https://github.com/user-attachments/assets/68b8e037-8cfc-4fe2-8eb8-da78dea7da37" />

로그인에 성공했기 때문에 다음과 같이 이름 확인이 가능하다.

<img width="337" height="192" alt="10  이름 확인" src="https://github.com/user-attachments/assets/17d3628f-a55c-4e29-b860-b5bec9bed0e4" />

로그아웃도 정상적으로 완료되었다.

<img width="341" height="181" alt="11  로그아웃 성공" src="https://github.com/user-attachments/assets/45e804b8-3af3-4c37-9e12-c49cc8361d4d" />

로그아웃 이후 이름 확인을 시도하면 실패한다.

<img width="339" height="199" alt="12  로그아웃 이후" src="https://github.com/user-attachments/assets/670c2b92-8c94-4686-ad0d-9d38e56b71fe" />

# 마무리

패스워드 해싱부터 시작해서 jwt 발급, 인증 방식까지 구현해보았다.
