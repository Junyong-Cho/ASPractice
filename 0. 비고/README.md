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
    byte[] content = await File.ReadAllBytesAsync("image.jpg");

    return Results.File(content, "image/jpg");
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