using System.Net.Http.Json;
using System.Reflection.Metadata;
using static System.Console;

HttpClient client = new();

while (true)
{
    string? ord = ReadLine();

    if (ord == "1")         // Post
    {
        Write("id 입력>> ");
        int id = int.Parse(ReadLine());
        Write("이름 입력>> ");
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
    }
    else if (ord == "2")    // Put
    {
        Write("id 입력>> ");
        int id = int.Parse(ReadLine());
        Write("이름 입력>> ");
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
    }
    else if (ord == "3")    // patch
    {
        Write("id 입력>> ");
        int id = int.Parse(ReadLine());
        Write("이름 변경 1/ 이메일 변경 2");
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
    }
    else if (ord == "4")    // Delete
    {
        Write("삭제할 id 입력>> ");
        int id = int.Parse(ReadLine());
        var response = await client.DeleteAsync($"http://localhost:5009/user/{id}");

        WriteLine(response.StatusCode);
    }
    else break;
}