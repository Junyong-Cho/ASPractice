using System.Net.Http.Json;
using static System.Console;

HttpClient client = new();

while (true)
{
    HttpResponseMessage response = null;
    Write(">> ");
    switch (ReadLine())
    {
        case "1":
            CreateDto cUser = new(input(), input(), input());

            response = await client.PostAsJsonAsync("http://localhost:5009/user",cUser);

            break;
        case "2":
            {
                int id = int.Parse(ReadLine());

                UpdateDto uUser = new(input(), input(), input());

                response = await client.PutAsJsonAsync($"http://localhost:5009/user/{id}", uUser);
            }
            break;
        case "3":
            {
                int id = int.Parse(ReadLine());
                
                UpdateDto uUser = new(input(), input(), input());

                response = await client.PatchAsJsonAsync($"http://localhost:5009/user/{id}", uUser);
            }
            break;
        case "4":
            {
                int id = int.Parse(ReadLine());

                response = await client.DeleteAsync($"http://localhost:5009/user/{id}");
            }
            break;
        default:
            return;
    }

    WriteLine($"\n{response.StatusCode}\n");

    if (!response.IsSuccessStatusCode)
    {
        WriteLine($"\n{await response.Content.ReadAsStringAsync()}\n");
    }
}

string? input()     // null 값을 입력받기 위해 정의한 메서드
{
    string? st = ReadLine();
    if (st == "null") return null;
    return st;
}

record CreateDto(string? Username, string? Password, string? Email);

record UpdateDto(string? Username, string? Password, string? Email);