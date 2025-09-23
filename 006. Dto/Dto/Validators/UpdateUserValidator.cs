using FluentValidation;

using Dto.Dtos;

namespace Dto.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.Username)                                        // null이 아닐 때만 비어있는지 검사
            .NotEmpty().WithMessage("비어 있으면 안됨")
            .When(u => u.Username != null);

        RuleFor(u => u.Password)                                        // null이면 건너뜀
            .Length(8, 30).WithMessage("8글자 이상")
            .Matches(@"[A-Z]").WithMessage("대문자 포함")
            .Matches(@"[a-z]").WithMessage("소문자 포함")
            .Matches(@"[0-9]").WithMessage("숫자 포함")
            .Matches(@"[!@#$.,]").WithMessage("지정된 특수문자 포함");

        RuleFor(u => u.Email)                                           // null이면 건너뜀
            .EmailAddress().WithMessage("올바르지 않은 이메일 형식");
    }
}
