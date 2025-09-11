# ��ȿ�� �˻�

���� Db�� ������ �������� ��ȿ���� �˻��ϴ� �ڵ带 �߰��غ��ڴ�.  
[Key], [Required] ���� �����Ϳ� �Ӽ��� �ο��� �־����� �̴� db�� ������ ������ ������ �� ���̸� �����͸� ������ ������ �ش� �����Ͱ� Null���� �̸��� �������� ��, ��ȿ�� �������� �˻������� �ʴ´�.  
�������� ��ȿ���� �˻����� �ʰ� Db�� ������ �õ��ϴ� ��쿡 db���� ȣ���� ���� ���� ���ܸ� ������ �Ǹ� ���� ������ ���ܸ� ó���ϱ� ���� �ڵ带 �߰��� �־�� �Ѵ�.  
try catch������ ���ܸ� ó���� �־ ������ db�� �����͸� ������ ���ܸ� ����޴� ��Ȳ�� �̻������� �ʴ�. (db���� �����͸� �����ϱ� ���� ��ȿ���� �˻��ϴ� ���� ����.)  
�׷��Ƿ� ���� ������ ���� �������� ��ȿ���� �˻��� �ִ� ���� ����.

## FluentValidation ���̺귯��
��ȿ���� �˻����ִ� ���̺귯���� ��ġ�غ��ڴ�. nuget�� FluentValidation(validator�� �򰥸��� �� �ȴ�.)�� �˻��Ѵ�.

![1. Fluent Validation](../dummy/6%20��ȿ��%20�˻�/1.%20FluentValidation.png)

FluentValidation�� �ؿ� �ִ� DependencyInjectionExtensions Ȯ���ѱ��� ���� ��ġ�� �ش�.

```dotnet list package```�� ��ġ�� Ȯ���Ѵ�.

## UserValidator ����
��ġ�� �Ϸ�Ǹ� User �������� ��ȿ���� �˻��� �� Validator�� ������ �� ���̴�.

������Ʈ�� Validators ���͸��� �����ϰ� �� ������ UserValidator.cs ������ ������ �ش�.

![2. ������Ʈ ����](../dummy/6%20��ȿ��%20�˻�/2.%20������Ʈ%20����.png)

UserValidator.cs ������ ������ ���� �ۼ��Ѵ�.

```C#
using FluentValidation;
using Validation.Models;

namespace Validation.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username).NotEmpty().WithMessage("�ʼ� �Է�");
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("�ʼ� �Է�")
            .Length(8,30).WithMessage("8 �̻� 30 ���� ����")
            .Matches(@"[A-Z]").WithMessage("�빮�� ����")
            .Matches(@"[a-z]").WithMessage("�ҹ��� ����")
            .Matches(@"[0-9]").WithMessage("���� ����")
            .Matches(@"[!@#$,.*]").WithMessage("������ Ư������ ����");
        RuleFor(u => u.Email).EmailAddress().WithMessage("�ùٸ��� ���� �̸��� ����");
    }
}
```

��ȿ���� �˻��ϰ� ���� �Ӽ��� NotEmpty EmailAddress Length Matches������ ������ �ش�.

## api���� �˻�

Post, Put, Patch �ܰ迡�� ���� ���� �����͸� ���� Ȥ�� �����ϱ� ���� ��ȿ�� �˻� �ڵ带 �ۼ��� �ش�.

```C#
app.MapPost("/user/{id}",async (UserDbContext db, IValidator<User> validator, User user, int id) =>
{
    var result = await validator.ValidateAsync(user);                           // ��ȿ�� �˻�
    
    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());                // ��ȿ���� ������ ������ �޼����� �Բ� ��ȯ

    if (await db.users.AnyAsync(u => u.Id == id)) return Results.Conflict();    // id ���� Ȯ��

    /*
    .....
    ..... db ���� �ڵ� �ۼ�
    */
    return Results.Created($"/user/{id}", user);                                // ���� �۵�
});
```

Put�� Post�� ���������� ��ȿ���� �˻��ϴ� �ڵ带 db�� ��û�ϱ� ���� �ۼ��� �� �ָ� �Ǵµ� Patch�� �������� �ʴ� �����͸� null�� ����α� ������ UserValidator�� �ƴ� PatchUserDtoValidator�� ���� Validator�� ������ �־�� �Ѵ�.

DTO(Data Transfer Object)�� ���� �ܰ迡 �� ���̹Ƿ� PatchValidator�� ���� �ܰ迡 ������ ������ �ϰڴ�.

# ������

��ȿ�� �˻� ����� �ϴ� �ڵ带 �߰��غ��Ҵ�.