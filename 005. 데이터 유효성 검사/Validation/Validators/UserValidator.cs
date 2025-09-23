using FluentValidation;
using Validation.Models;

namespace Validation.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username).NotEmpty().WithMessage("필수 입력");
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("필수 입력")
            .Length(8,30).WithMessage("8 이상 30 이하 문자")
            .Matches(@"[A-Z]").WithMessage("대문자 포함")
            .Matches(@"[a-z]").WithMessage("소문자 포함")
            .Matches(@"[0-9]").WithMessage("숫자 포함")
            .Matches(@"[!@#$,.*]").WithMessage("지정된 특수문자 포함");
        RuleFor(u => u.Email).EmailAddress().WithMessage("올바르지 않은 이메일 형식");
    }
}
