# ����

�츮�� ���ݱ��� User ���� ��û�� ������ ������ְ� id�� ���� ��ȸ���ִ� ��ɱ����� �����ߴ�.  

���� ����� ������ �α��� ����� �����ڴ�.

��� ������ Id�� �н����带 �Է��ϸ� db���� �ش� ID�� ������ �����ϴ��� Ȯ���ϰ� �����ϸ� �н����尡 ������ Ȯ���� ������ �α��� ���� ���θ� ��ȯ�ϵ��� �غ��ڴ�.

## SignupUserDto, SigninUserDto ����

���� ���� �����͸� �ְ���� Dto�� ������ְڴ�. SignupUserDto�� CreateUser�� ������ ��������� ȸ�������� ������ �����ϴ� ����̹Ƿ� Create���� Sign up���� ����ϴ� ���� �� ���� �� ���Ƽ� CreateUserDto ������ ������ ������ ������ ���ο� SignupUserDto�� �����ڴ�.

������ ���� Dto���� ���� �����ϵ��� Dtos ���丮 ������ AuthDtos ���丮�� ���� ���� ������ SigninUserDto.cs�� SingupUserDto.cs ������ �����.

![1. Auth ���͸�](../.dummy/10%20����/1.%20Auth%20���͸�.png)

�� ���� SignupUserDto Ŭ������ ������ ���� �ۼ��Ѵ�.  
������ �ۼ��ߴ� CreateUserDto�� ����.  

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

�׸��� �α����� �� ���Ǵ� SigninUserDto�� ������ ���� �ۼ��Ѵ�.  
�α��ο��� UserId�� �н����常 ���� ���̴�.

```C#
namespace Authentication.Dtos.AuthDtos;

public class SigninUserDto
{
    public string UserId { get; set; } = null!;
    public string Password { get; set; } = null!;
}
```

## UserId �ε��� ����

SigninUserDto�� ���Ե� �����ʹ� ���� id�� �н����� ���̴�.  
���ݱ��� db���� ��ƼƼ�� ã�� �� ��ƼƼ�� �⺻Ű�� ��ȸ�ؼ� ã�Ҵµ� �⺻Ű�� db���� �ڵ����� �����ϴ� ��ȣ�̹Ƿ� �α����� ���� UserId�� ��ȸ�ϵ��� �� ���̴�.  

���� id�� ��ƼƼ�� ��ȸ�ϴ� ����� ������ ����.

```C#
app.MapGet("/{id}", async (UserDbContext db, int id) =>
{
    User? user = await db.Users.FindAsync(id);
    /*�⺻Ű�� ��ȸ*/
});

app.MapGet("/{userId}", async (UserDbContext db, string userId) =>
{
    User? user = await db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

    /*�⺻Ű�� �ƴ� �Ӽ����� ��ȸ*/
});
```

�׷��� �⺻Ű�� �ƴ� �Ӽ����� ��ȸ�ϰ� �Ǹ� �����̼��� ��� UserId ���� ��ȸ�ؼ� ��Ī�Ǵ� ���� �ִ��� Ȯ���غ��� �ϹǷ� ���� �������� �ʴ� UserId���(�� �־��� �����) ��ƼƼ Ž���� �ð����⵵�� O(n)�� �ȴ�.

���� ȿ�������� UserId�� ��ȸ�ϱ� ���� User �����̼��� UserId �Ӽ��� �ε����� �����ؼ� ��ȸ�� �־��� ��쿡�� O(log n)�� �ɸ����� �����ϰڴ�.

### UserDbContext �������̵�

UserDbContext.cs ������ ������ ���� �����ߴ�.
```C#
public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    //�������̵� �߰�
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users")    // �����̼� �̸� ����
            .HasIndex(u => u.UserId).IsUnique();        // UserId �ε��� �� Unique ����
    }
}
```

�ε����� �����ϴ� �迡 �����̼� �̸��� users�� ���������.  
UserDbContext�� DbSet�� �̸��� C# ��� ��ӿ� �°� Users�� �������־��� ������ �� ������ ������ db�� Users��� �̸��� �����̼��� �����Ǵµ� �̴� SQL������ ��ȸ�� �� select * from "Users"��� ����ǥ ǥ�ø� �� �־�� �ϴ� �������� �ֱ⿡ �ҹ��ڷ� �������ذ��̴�.

�׸��� HasIndex()�� UserId�� �ε����� �������ش�.  
�̷��� UserId �Ӽ��� db���� ���ĵǸ鼭 ����Ž������ O(log n)�� Ž���� ����������.  
���Ͽ� UserId�� �ߺ��Ǹ� �� �ǹǷ� IsUnique()�� �ߺ� �Ұ� ������ ���־���.

db ������ �����Ͽ����Ƿ� ```dotnet ef migrations add (���� ����)``` ```dotnet ef database update``` ��ɾ�� db�� ������Ʈ���ش�.

## �н����� �ؽ�

�Ϲ������� ���� �����͸� ������ �� �н������ �ؽ��ؼ� �����Ѵ�.  
�̹� �ܰ迡���� �׷��� �� �� ���̴�.

���� nuget ����Ʈ���� BCrypt�� �˻��ؼ� BCrypt.Net-Next�� ��ġ�Ѵ�.

![2. B Crypt Net Next](../.dummy/10%20����/2.%20BCrypt%20Net%20Next.png)

�׸��� signup ��û�� ������ ������ �޼��带 ������ ���� �������ش�.

```C#
app.MapPost("/signup", async (UserDbContext db, SignupUserDto signupUser, IValidator<SignupUserDto> validator, IMapper mapper) =>
{
    var results = validator.Validate(signupUser);       // ��ȿ�� �˻�

    if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

    if (await db.Users.AnyAsync(u => u.UserId == signupUser.UserId)) return Results.Conflict();     // id �ߺ� �˻�

    User user = mapper.Map<User>(signupUser);

    user.HashPassword = BCrypt.Net.BCrypt.HashPassword(signupUser.Password);        // �н����� �ؽ�

    db.Users.Add(user);

    await db.SaveChangesAsync();

    return Results.NoContent();
});
```

## JWT(Json Web Token)

�α��ο� ������ ������ ���ϴ� ���񽺸� ��û�� �� �ִ�.  
�׸��� ������ ������ ���񽺸� ��û�� �� ���� ������ �ִ���(�α��ο� ������ ��������) Ȯ���ؾ� �Ѵ�.  
������ ������ ������ Ȯ���ϴ� ����� ���� ������ �ִµ� ���⼭�� ��ū�� ����Ͽ� ������ Ȯ���ϵ��� �غ��ڴ�.

��ū�� ������ ������ ������ �������� �ο��ϴ� Ȯ���� ���� �Ŷ�� �� �� �ִ�.  
�α��ο� ������ �������� ������ ��ū�� �����ְ� ������ � ���񽺸� �̿��ϰ��� �� �� ��ū�� �Բ� ��û�� ������ ������ �� ��ū�� �ڽ��� �ο��� ���� �´��� �׸��� ��ȿ�� ��ū���� Ȯ���� ������ ���񽺸� �����ϰ� �ȴ�.

���� nuget ����Ʈ�� �����ؼ� Microsoft.AspNetCore.Authentication.JwtBearer�� ��ġ�Ѵ�.

![3. Jwt Bearer](../.dummy/10%20����/3.%20JwtBearer.png)

�׸��� ���� ������ ���� ��Ī Ű�� �������ְڴ�.  
openssl�� �̿��Ͽ� ������ �־ �ǰ� C# �ڵ�� �����ϰ� �������־ �ȴ�.  
������ ���� openssl�� ��ġ�� �ؾ� �ϹǷ� C# �ڵ�� 128����Ʈ ���̷� ������ ���ڴ�.

```C#
using System.Security.Cryptography;
using static System.Console;

byte[] key = new byte[128];

RandomNumberGenerator.Fill(key);

Write(Convert.ToBase64String(key));
```

���� �ڵ带 �����ϸ� �������� ������ Ű�� base64�� ���ڵ��� ����� ���� �� �ִ�.  
�� Ű�� secrets.json ���Ͽ� ������ ���� ������� �߰��Ѵ�.

```json
"Jwt" : {"Key" :"G+bRj/IJg5K9/lvnrnyQEBxo1sJ3jbdU67ojyNggUxtjuqR202WzmByA8qxoZWj6CocDM1QU5Io0WJHi7IekCyVCQCuSLK/gBJ/cmGK1VfxZxtTOlPUN49mzuwNzbffekon/hk7MZhmRC2yFeZaAHtynvLXVg4z3y2brqwYx7D4="}
            // �������� ������ Ű
```

���� �α��� ��û�� ������ ���� �� ��ū�� �߱��ϴ� �ڵ带 �ۼ��غ��ڴ�.

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

    JwtSecurityToken token = new(               // ��ū ����
        issuer: config["Issuer"],               // ��ū �߱��� (���� null)
        audience: config["Audience"],           // ��ū�� �޴� Ŭ���̾�Ʈ (���� null)
        claims: claims_,                        // ����� ����
        signingCredentials: credential,         // ������
        expires: DateTime.Now.AddMinutes(60));  // ��ū ��ȿ�ð� 60��

    string stringToken = new JwtSecurityTokenHandler().WriteToken(token);

    CookieOptions cookieOptions = new()
    {
        HttpOnly = true,                        // ������������ �ڵ����� ��Ű ó���ϱ�
        Secure = true,                          // https �������ݸ� ���
        Expires = DateTime.Now.AddMinutes(60)   // ��ū ��ȿ �Ⱓ 60��
    };

    context.Response.Cookies.Append("jwt-token", stringToken, cookieOptions);
    // HttpContext ��Ű�� ��ū �߰�

    return Results.Ok(new { Message = "SUCCESS!" });
});
```
�α����� �õ����� �� ��� �������� ģ���� �˷��ִ� ���� ���Ȼ� ������ ���� �Ŷ�� ������ ��ȿ�� ������ �߻����� ������ Unathorized�� ��ȯ�ϵ��� �Ͽ���.

�׸��� ��ū�� ������ �� Ŭ���̾�Ʈ������ ��ū�� ó���ϱ⺸�� ���������� �ڵ����� ó���ϵ���(�� ���������� javascript�� ó������ �ʵ���) ��Ű �ɼ��� �����ϰ� HttpContext�� �����Ͽ� �����ϵ��� �ۼ�����.

�� ��Ű �ɼǿ� ```Secure = true``` �κ��� https �������ݸ� ó���ϵ��� �ϴ� �ɼ��̹Ƿ� appsettings.json ���Ͽ� ���� �ڵ带 �߰��Ѵ�.

```json
"Urls" : "https://localhost:5009" // ��Ʈ ��ȣ ����
```

���������� �α׾ƿ� �ڵ�� ������ ����.

```C#
app.MapPost("/signout", (HttpContext context) =>
{
    context.Response.Cookies.Delete("jwt-token");
    return Results.Ok(new { Message = "SUCCESS" });
}).RequireAuthorization();
```

## ��ū ���� ��û ó��

���� �������� ��ū�� Ȯ���ϵ��� ������ ���ְڴ�.

```C#
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,    // ������ ����
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:Key"]!)), // ������ Ű
            ValidateIssuer = false,             // Issuer ���� ����
            ValidateAudience = false            // Audience ���� ����
        };

        options.Events = new()                  // ���������� �ڵ����� ó���� ��ū�� ����
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("jwt-token"))       // �������� ���� ��ū�� �����ϸ�
                    context.Token = context.Request.Cookies["jwt-token"];   // ������ Ȯ���ؾ� �ϴ� ��ū ����
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
```

���� ��ū�� �����ϴ� ���������� ��û�� ���� ��쿡�� ��û�� ó�����ִ� ��������Ʈ�� �����ϰ� �����ڴ�.

```C#
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/hello", () => "Hello user").RequireAuthorization();
```

���� ���� �������� ������ �� Authentication�� Authorization�� �߰����ְ� ��û ���� ���� �޼��� �ڿ� RequireAuthorization() �޼��带 �߰����ָ� ��û�� ó���ϱ� ���� ���� ���θ��� Ȯ���� ���̴�.

�׸��� ȸ�����԰� �α��� ���� gemini���� �ۼ����״�.  
�α��ο� �ʿ��� �����ʹ� UserId�� Password�̰� /auth/signin���� �������� �Ͽ���  
ȸ�����Կ� �ʿ��� �����ʹ� UserId, Password, Email, Username�̰� /auth/signup���� �������� �ۼ��϶� �ߴ�.  
���������� �α��ο� ������ ������ Ŭ���ϸ� ȯ���մϴٸ� ����ϵ��� �ϴ� ��ư�� �α׾ƿ� ��ư�� �߰��ߴ�.

gemini�� �ۼ����� �ڵ���� ������ ����.

index.html
```html
<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>�α��� / ȸ������</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>

    <div class="container">
        <!-- ȸ������ �� -->
        <div id="signup-section">
            <h2>ȸ������</h2>
            <form id="signup-form">
                <div class="form-group">
                    <label for="reg-userid">����� ID</label>
                    <input type="text" id="reg-userid" required>
                </div>
                <div class="form-group">
                    <label for="reg-username">�̸�</label>
                    <input type="text" id="reg-username" required>
                </div>
                <div class="form-group">
                    <label for="reg-email">�̸���</label>
                    <input type="email" id="reg-email" required>
                </div>
                <div class="form-group">
                    <label for="reg-password">��й�ȣ</label>
                    <input type="password" id="reg-password" required>
                </div>
                <button type="submit">�����ϱ�</button>
            </form>
        </div>

        <hr>

        <!-- �α��� �� -->
        <div id="signin-section">
            <h2>�α���</h2>
            <form id="signin-form">
                <div class="form-group">
                    <label for="login-userid">����� ID</label>
                    <input type="text" id="login-userid" required>
                </div>
                <div class="form-group">
                    <label for="login-password">��й�ȣ</label>
                    <input type="password" id="login-password" required>
                </div>
                <button type="submit">�α���</button>
            </form>
        </div>

        <hr>

        <!-- ���� �׽�Ʈ ���� -->
        <div id="test-section">
            <h2>���� �׽�Ʈ</h2>
            <button id="myname-button">�� �̸� Ȯ���ϱ�</button>
            <button id="signout-button" class="secondary">�α׾ƿ�</button>
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

    // �޽��� ǥ�� �Լ�
    const showMessage = (text, type) => {
        messageDiv.textContent = text;
        messageDiv.className = type; // 'success' �Ǵ� 'error'
    };

    // ȸ������ �� ���� �̺�Ʈ
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
                    showMessage('ȸ������ ����!', 'success');
                    signupForm.reset();
                } else {
                    const errorData = await response.json();
                    const firstError = errorData.errors ? Object.values(errorData.errors)[0][0] : (errorData.message || response.statusText);
                    showMessage(`ȸ������ ����: ${firstError}`, 'error');
                }
            } catch (error) {
                showMessage('��Ʈ��ũ ������ �߻��߽��ϴ�.', 'error');
            }
        });
    }

    // �α��� �� ���� �̺�Ʈ
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
                    showMessage('�α��� ����! ���� ��ư�� �׽�Ʈ�غ�����.', 'success');
                } else {
                    showMessage('�α��� ����: ID �Ǵ� ��й�ȣ�� Ȯ���ϼ���.', 'error');
                }
            } catch (error) {
                showMessage('��Ʈ��ũ ������ �߻��߽��ϴ�.', 'error');
            }
        });
    }

    // ���� �׽�Ʈ ��ư Ŭ�� �̺�Ʈ
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
                    showMessage(`���� ����: ${response.status} (�α����� �ʿ��մϴ�)`, 'error');
                }
            } catch (error) {
                showMessage('��Ʈ��ũ ������ �߻��߽��ϴ�.', 'error');
            }
        });
    }

    // �α׾ƿ� ��ư Ŭ�� �̺�Ʈ
    if (signoutButton) {
        signoutButton.addEventListener('click', async () => {
            try {
                const response = await fetch('/auth/signout', {
                    method: 'POST'
                });

                if (response.ok) {
                    showMessage('�α׾ƿ� �Ǿ����ϴ�.', 'success');
                } else {
                    showMessage('�α׾ƿ� ����. (�α��� ���°� �ƴ� �� �ֽ��ϴ�)', 'error');
                }
            } catch (error) {
                showMessage('��Ʈ��ũ ������ �߻��߽��ϴ�.', 'error');
            }
        });
    }
});
```

���� wwwroot, wwwroot/css, wwwroot/js�� ����ְ� ���� �ڵ带 ���� �����ϵ��� �ߴ�.

```C#
app.UseStaticFiles();

app.MapGet("/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/index.html");
});
```

���������� �α��ο� ������ ������ ��ư�� ������ �����ϴ� �޼��带 �ۼ��ߴ�.

```C#
app.MapGet("/myname", async (HttpContext context, UserDbContext db) =>
{
    string? id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    User? user = await db.Users.FindAsync(int.Parse(id!));

    return $"ȯ���մϴ�. {user.Username}��";
}).RequireAuthorization();
```

## �׽�Ʈ

���� ������Ʈ�� �����ϰ� ```https://localhost:5009```�� �����Ѵ�. (https�� ����� �־�� �Ѵ�.)

![4. ����������](../.dummy/10%20����/4.%20����������.png)

�α����� �� �ϸ� ����ó�� ������ �����Ѵ�.  

![5. �̸� ����](../.dummy/10%20����/5.%20�̸�%20����.png)  
![6. �α׾ƿ� ����](../.dummy/10%20����/6.%20�α׾ƿ�%20����.png)

��Ŀ� �°� ������ �Է��ϸ� ȸ�����Կ� �����Ѵ�.

![7. ȸ������ ����](../.dummy/10%20����/7.%20ȸ������%20����.png)

���� db�� �����غ��� �н����� �ؽ̱��� �� �� ���� Ȯ���� �� �ִ�.

![8. ����Ȯ��](../.dummy/10%20����/8.%20����Ȯ��.png)

ȸ�������� ������� �α����ϸ� �α��ο� �����Ѵ�.

![9. �α��� ����](../.dummy/10%20����/9.%20�α���%20����.png)

�α��ο� �����߱� ������ ������ ���� �̸� Ȯ���� �����ϴ�.

![10. �̸� Ȯ��](../.dummy/10%20����/10.%20�̸�%20Ȯ��.png)

�α׾ƿ��� ���������� �Ϸ�Ǿ���.

![11. �α׾ƿ� ����](../.dummy/10%20����/11.%20�α׾ƿ�%20����.png)

�α׾ƿ� ���� �̸� Ȯ���� �õ��ϸ� �����Ѵ�.

![12. �α׾ƿ� ����](../.dummy/10%20����/12.%20�α׾ƿ�%20����.png)

# ������

�н����� �ؽ̺��� �����ؼ� jwt �߱�, ���� ��ı��� �����غ��Ҵ�.