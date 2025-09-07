using System.Net.Http.Json;
using System.Reflection.Metadata;
using static System.Console;

HttpClient client = new();

while (true)
{
    string? ord = ReadLine();

    if (ord == "1")
    {
        var user = new
        {
            Id = 1010,
            Username = "Choi",
            Email = "Choi@example.com"
        };

        var response = await client.PostAsJsonAsync($"http://localhost:5030/user/{user.Id}", user);

        WriteLine(response.StatusCode);
    }
    else if (ord == "2")
    {
        var user = new
        {
            Id = 1111,
            Username = "Hong",
            Email = "Hong@example.com"
        };

        var response = await client.PutAsJsonAsync($"http://localhost:5030/user/{user.Id}", user);

        WriteLine(response.StatusCode);
    }
    else if (ord == "3")
    {
        var response = await client.DeleteAsync("http://localhost:5030/user/1010");

        WriteLine(response.StatusCode);
    }
    else break;
}
