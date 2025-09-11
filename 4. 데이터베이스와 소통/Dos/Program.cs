HttpClient client = new();

//string url = "http://localhost:5009/sync";      // 동기 url
string url = "http://localhost:5009/async";     // 비동기 url

List<Task<HttpResponseMessage>> tasks = new();

for (int i = 0; i < 100; i++)                   // 100번 동시 요청 Tasks 생성
    tasks.Add(client.GetAsync(url));

Console.WriteLine("\nStart\n");

await Task.WhenAll(tasks);                      // 동시 요청 시작

Console.WriteLine("\nEnd\n");