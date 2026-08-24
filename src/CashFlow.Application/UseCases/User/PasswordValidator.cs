using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application.UseCases.User.Register;

public partial class PasswordValidator<T>: PropertyValidator<T, string>
{
    private const string ERROR_MESSAGE_KEY = "ErrorMessage";
    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_MESSAGE_KEY}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if(string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, "Password is required.");
            return false;
        }

        if(password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, "Password must be at least 8 characters long.");
            return false;
        }
        
        var hasUpperCase = UpperCase().IsMatch(password);
        var hasLowerCase = LowerCase().IsMatch(password);
        var hasDigit = Digit().IsMatch(password);
        var hasSpecialChar = SpecialChar().IsMatch(password);

        if(!hasUpperCase || !hasLowerCase || !hasDigit || !hasSpecialChar)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
            return false;
        }

        return true;
    }

    [GeneratedRegex("[A-Z]")]
    private static partial Regex UpperCase();
    [GeneratedRegex("[a-z]")]
    private static partial Regex LowerCase();
    [GeneratedRegex("[0-9]")]
    private static partial Regex Digit();
    [GeneratedRegex("[^a-zA-Z0-9]")]
    private static partial Regex SpecialChar();
}