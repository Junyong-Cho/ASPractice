using Dto.Dtos;
using FluentValidation;

namespace Dto.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Username)                                        // null 불허
            .NotNull().WithMessage("필수 입력");

        RuleFor(u => u.Password)                                        // null 불허
            .NotNull().WithMessage("필수 입력")
            .Length(8, 30).WithMessage("8글자 이상")                    // 글자 수 제한
            .Matches(@"[a-z]").WithMessage("소문자 포함")               // 소문자, 대문자, 숫자, 특수문자 검사
            .Matches(@"[A-Z]").WithMessage("대문자 포함")
            .Matches(@"[0-9]").WithMessage("숫자 포함")
            .Matches(@"[!@#$.]").WithMessage("특정한 특수문자 포함");

        RuleFor(u => u.Email).EmailAddress().WithMessage("올바르지 않은 이메일 형식"); // null이면 건너뜀
    }
}
