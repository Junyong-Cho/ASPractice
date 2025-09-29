using Authentication.Dtos.AuthDtos;
using FluentValidation;

namespace Authentication.Validators;

public class SignupUserValidator : AbstractValidator<SignupUserDto>
{
    public SignupUserValidator()
    {
        RuleFor(u => u.UserId)
            .NotNull().WithMessage("필수 입력")
            .Length(8,30).WithMessage("8글자 이상 30글자 이하");
        RuleFor(u => u.Password)
            .NotNull().WithMessage("필수 입력")
            .Length(10, 30).WithMessage("8글자 이상 30글자 이하")
            .Matches(@"[a-z]").WithMessage("소문자 포함")
            .Matches(@"[A-Z]").WithMessage("대문자 포함")
            .Matches(@"[0-9]").WithMessage("숫자 포함")
            .Matches(@"[,./!@#$]").WithMessage("지정된 특수문자 포함");

        RuleFor(u => u.Email)
            .NotNull().WithMessage("필수 입력")
            .EmailAddress().WithMessage("올바르지 않은 이메일 형식");

        RuleFor(u => u.Username)
            .NotNull().WithMessage("필수 입력")
            .Length(2, 10).WithMessage("10 글자 이하");
    }
}
