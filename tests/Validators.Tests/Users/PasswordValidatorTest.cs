using CashFlow.Application.UseCases.User.Register;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("short")]
    [InlineData("alllowercase")]
    [InlineData("ALLUPPERCASE")]
    [InlineData("12345678")]
    [InlineData("password")]
    [InlineData("passwordA")]
    [InlineData("Password1")]
    [InlineData("Password!")]
    [InlineData(null)]
    public void Error_Password_Invalid(string? password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson { Password = password! }), password!);
        Assert.False(result);
    }
}