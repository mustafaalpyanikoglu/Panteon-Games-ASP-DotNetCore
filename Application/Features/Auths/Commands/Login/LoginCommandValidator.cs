using FluentValidation;
using static Application.Features.Auths.Constants.AuthMessages;

namespace Application.Features.Auths.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.UserForLoginDto.Username)
            .NotEmpty()
            .WithMessage(UsernameMustNotBeEmpty)
            .MinimumLength(3)
            .WithMessage(UsernameMinimumNumberOfCharacters);

        RuleFor(c => c.UserForLoginDto.Password)
                .NotEmpty()
                .WithMessage(PasswordMustNotBeEmpty)
                .MinimumLength(6)
                .WithMessage(PasswordMinimumNumberOfCharacters);
    }
}
