using Authentication.Dtos.AuthDtos;
using FluentValidation;

namespace Authentication.Validators;

public class SigninUserValidator : AbstractValidator<SigninUserDto>
{
    public SigninUserValidator()
    {
        RuleFor(u => u.UserId)
            .NotNull()
            .Length(8, 30);
        RuleFor(u => u.Password)
            .NotNull()
            .Length(10, 30)
            .Matches(@"[a-z]")
            .Matches(@"[A-Z]")
            .Matches(@"[0-9]")
            .Matches(@"[,./!@#$]");
    }
}
