using FluentValidation;
using static Application.Features.Auths.Constants.AuthMessages;

namespace Application.Features.Auths.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.UserForRegisterDto.Email)
            .NotEmpty()
            .WithMessage(EmailMustNotBeEmpty)

            .EmailAddress()
            .WithMessage(YourMailIsNotAvailable);

        RuleFor(c => c.UserForRegisterDto.Username)
            .NotEmpty()
            .WithMessage(UsernameMustNotBeEmpty)
            .MinimumLength(3)
            .WithMessage(UsernameMinimumNumberOfCharacters);

        RuleFor(c => c.UserForRegisterDto.Password)
                .NotEmpty()
                .WithMessage(PasswordMustNotBeEmpty)
                .MinimumLength(6)
                .WithMessage(PasswordMinimumNumberOfCharacters)
                .Must(ContainUppercase)
                .WithMessage(PasswordMustContainUppercaseLetters)
                .Must(ContainLowercase)
                .WithMessage(PasswordMustContainLowercaseLetters)
                .Must(ContainDigit)
                .WithMessage(PasswordMustContainNumber)
                .Must(ContainSymbol)
                .WithMessage(PasswordMustContainSymbol);

    }
    private bool ContainUppercase(string password) => password.Any(c => char.IsUpper(c));

    private bool ContainLowercase(string password) => password.Any(c => char.IsLower(c));

    private bool ContainDigit(string password) => password.Any(c => char.IsDigit(c));

    private bool ContainSymbol(string password) => password.Any(c => !char.IsLetterOrDigit(c));

}
