using System.Net.Http.Json;

HttpClient client = new();

User user1, user2, user3, user4;

user1 = new("", "Password!12", "email@example.com");        // 비어 있는 이름
user2 = new("username", "21312321", "email@example.com");   // 올바르지 않은 패스워드
user3 = new("username", "Password!12", "emailexamplecom");  // 올바르지 않은 이메일 형식

user4 = new("", "", "");                                    // 전부 오류

var re1 = await client.PostAsJsonAsync($"http://localhost:5009/user/{1}", user1);
var re2 = await client.PostAsJsonAsync($"http://localhost:5009/user/{1}", user2);
var re3 = await client.PostAsJsonAsync($"http://localhost:5009/user/{1}", user3);

var re4 = await client.PostAsJsonAsync($"http://localhost:5009/user/{1}", user4);

Console.WriteLine(await re1.Content.ReadAsStringAsync());
Console.WriteLine();
Console.WriteLine(await re2.Content.ReadAsStringAsync());
Console.WriteLine();
Console.WriteLine(await re3.Content.ReadAsStringAsync());
Console.WriteLine();
Console.WriteLine(await re4.Content.ReadAsStringAsync());

public record User(string Username, string Password, string Email);