namespace Application.Features.Auths.Constants;

public static class AuthMessages
{
    public const string EmailAuthenticatorDontExists = "Email authenticator don't exists.";
    public const string OtpAuthenticatorDontExists = "Otp authenticator don't exists.";
    public const string AlreadyVerifiedOtpAuthenticatorIsExists = "Already verified otp authenticator is exists.";
    public const string EmailActivationKeyDontExists = "Email Activation Key don't exists.";
    public const string UserDontExists = "User don't exists.";
    public const string UserHaveAlreadyAAuthenticator = "User have already a authenticator.";
    public const string RefreshDontExists = "Refresh don't exists.";
    public const string InvalidRefreshToken = "Invalid refresh token.";
    public const string UserEmailAlreadyExists = "User email already exists";
    public const string UsernameAlreadyExists = "Username already exists";
    public const string PasswordDontMatch = "Password don't match.";
    public const string PasswordChangedSuccessfully = "Şifre değiştirildi";
    public const string PasswordDoNotMatch = "passwords do not match";

    public const string YourMailIsNotAvailable = "Your mail is not available";
    public const string UsernameMinimumNumberOfCharacters = "Username must be a minimum of 3 characters";
    public const string EmailMinimumNumberOfCharacters = "Email must be a minimum of 4 characters";
    public const string PasswordMinimumNumberOfCharacters = "Password must be at least 6 characters.";
    public const string PasswordMustContainUppercaseLetters = "Password must contain uppercase letters.";
    public const string PasswordMustContainLowercaseLetters = "Password must contain lowercase letters.";
    public const string PasswordMustContainSymbol = "Password must contain symbol.";
    public const string PasswordMustContainNumber = "Password must contain numbers";
    public const string ProjectTaskStatusCannotBeNull = "Project task status can not be null.";
    public const string SecurityCodeCannotBeEmpty = "Security code cannot be empty";
    public const string UsernameMustNotBeEmpty = "Username must not be empty!";
    public const string PasswordMustNotBeEmpty = "Password must not be empty!";
    public const string EmailMustNotBeEmpty = "Email must not be empty!";
}
