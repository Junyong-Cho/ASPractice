## html 파일 전송법

```C#
app.MapGet("/file", async () =>
{
    string content = await File.ReadAllTextAsync("filepath.html");

    return Results.Content(content, "text/html");
});
```

## 이미지 파일 전송법

```C#
app.MapGet("/image", async () =>
{
    byte[] content = await File.ReadAllBytesAsync("image.jpg");     // 이미지 파일을 바이트 값으로 저장

    return Results.File(content, "image/jpg");
});
```

또는
```C#
app.MapGet("/image", () =>
{
    string path = "image.jpg";                                          // 상대 경로를
    string file = Path.Combine(Directory.GetCurrentDirectory(), path);  // 절대 경로로 전환

    return Results.File(file, "image/jpg");
});
```

## 비디오 스트리밍 방법

```C#
app.MapGet("/video", async () =>
{
    byte[] files = await File.ReadAllBytesAsync("video.mp4");               // 비디오 파일을 바이트 값으로 저장

    return Results.File(files, "video/mp4", enableRangeProcessing: true);
                                            // enableRangeProcessing이 참이면 비디오 진행 바 조절 가능
                                            // 거짓이면 스트리밍은 되지만 비디오 진행 바 조절 불가능
});
```

또는

```C#
app.MapGet("/video", () =>
{
    string path = "video.mp4";                              // 상대 경로를
    byte[] files = await File.ReadAllBytesAsync(path);      // 절대 경로로 전환

    return Results.File(files, "video/mp4", enableRangeProcessing: true);
                                            // enableRangeProcessing이 참이면 비디오 진행 바 조절 가능
                                            // 거짓이면 스트리밍은 되지만 비디오 진행 바 조절 불가능
});
```


## Url:포트번호 설정법
appsettings.json 파일에 다음과 같은 ```키:값```을 추가한다.
```json
"Urls": "https://localhost:8080",
```


## DB 자동생성 키 값 초기화 방법

```ALTER SEQUENCE "테이블이름_키속성이름_seq" with restart 1;``` => 키 갑을 1로 초기화(대문자가 있으면 따옴표 표시 필수)  
```SELECT nextval('"테이블이름_키속성이름_seq"');``` => 다음으로 지정될 키 값 확인('와 "에 주목)

## 마이그레이션 이력 조회

```dotnet ef migrations list``` 명령어로 조회

## 마이그레이션 업데이트 되돌리기

조회한 마이그레이션 이력에서 지우고 싶은 이력 위에 마이그레이션으로 db 업데이트하기

```dotnet ef database update <마이그레이션 이름>```

```dotnet ef migrations remove``` 명령어로 가장 최근에 생성된 마이그레이션부터 제거

## 마이그레이션 전체 초기화

```dotnet database update 0```으로 db 초기화  
그리고 InitialCreate 단계가 삭제될 때까지 ```dotnet ef migrations remove``` 명령어 실행