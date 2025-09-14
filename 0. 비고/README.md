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
    byte[] content = await File.ReadAllBytesAsync("image.jpg");

    return Results.File(content, "image/jpg");
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