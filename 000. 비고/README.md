## html ���� ���۹�

```C#
app.MapGet("/file", async () =>
{
    string content = await File.ReadAllTextAsync("filepath.html");

    return Results.Content(content, "text/html");
});
```

## �̹��� ���� ���۹�

```C#
app.MapGet("/image", async () =>
{
    byte[] content = await File.ReadAllBytesAsync("image.jpg");     // �̹��� ������ ����Ʈ ������ ����

    return Results.File(content, "image/jpg");
});
```

�Ǵ�
```C#
app.MapGet("/image", () =>
{
    string path = "image.jpg";                                          // ��� ��θ�
    string file = Path.Combine(Directory.GetCurrentDirectory(), path);  // ���� ��η� ��ȯ

    return Results.File(file, "image/jpg");
});
```

## ���� ��Ʈ���� ���

```C#
app.MapGet("/video", async () =>
{
    byte[] files = await File.ReadAllBytesAsync("video.mp4");               // ���� ������ ����Ʈ ������ ����

    return Results.File(files, "video/mp4", enableRangeProcessing: true);
                                            // enableRangeProcessing�� ���̸� ���� ���� �� ���� ����
                                            // �����̸� ��Ʈ������ ������ ���� ���� �� ���� �Ұ���
});
```

�Ǵ�

```C#
app.MapGet("/video", () =>
{
    string path = "video.mp4";                              // ��� ��θ�
    byte[] files = await File.ReadAllBytesAsync(path);      // ���� ��η� ��ȯ

    return Results.File(files, "video/mp4", enableRangeProcessing: true);
                                            // enableRangeProcessing�� ���̸� ���� ���� �� ���� ����
                                            // �����̸� ��Ʈ������ ������ ���� ���� �� ���� �Ұ���
});
```


## Url:��Ʈ��ȣ ������
appsettings.json ���Ͽ� ������ ���� ```Ű:��```�� �߰��Ѵ�.
```json
"Urls": "https://localhost:8080",
```


## DB �ڵ����� Ű �� �ʱ�ȭ ���

```ALTER SEQUENCE "���̺��̸�_Ű�Ӽ��̸�_seq" with restart 1;``` => Ű ���� 1�� �ʱ�ȭ(�빮�ڰ� ������ ����ǥ ǥ�� �ʼ�)  
```SELECT nextval('"���̺��̸�_Ű�Ӽ��̸�_seq"');``` => �������� ������ Ű �� Ȯ��('�� "�� �ָ�)

## ���̱׷��̼� �̷� ��ȸ

```dotnet ef migrations list``` ��ɾ�� ��ȸ

## ���̱׷��̼� ������Ʈ �ǵ�����

��ȸ�� ���̱׷��̼� �̷¿��� ����� ���� �̷� ���� ���̱׷��̼����� db ������Ʈ�ϱ�

```dotnet ef database update <���̱׷��̼� �̸�>```

```dotnet ef migrations remove``` ��ɾ�� ���� �ֱٿ� ������ ���̱׷��̼Ǻ��� ����

## ���̱׷��̼� ��ü �ʱ�ȭ

```dotnet database update 0```���� db �ʱ�ȭ  
�׸��� InitialCreate �ܰ谡 ������ ������ ```dotnet ef migrations remove``` ��ɾ� ����