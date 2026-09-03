using CashFlow.Application.UseCases.User.Register;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Users.Register;


public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Error_Name_Empty(string? name)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Name = name!;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);

        result.Errors.ShouldNotBeEmpty();
        result.Errors.ShouldContain(e => e.PropertyName == "Name");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Error_Email_Empty(string? email)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Email = email!;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);

        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.PropertyName == "Email");
        result.Errors.ShouldContain(e => e.ErrorMessage == "Email is required.");
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Email = "invalid-email";

        var result = validator.Validate(request);

        Assert.False(result.IsValid);

        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.PropertyName == "Email");
        result.Errors.ShouldContain(e => e.ErrorMessage == "Invalid email format.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Error_Password_Empty(string? password)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Password = password!;

        var result = validator.Validate(request);

        Assert.False(result.IsValid);

        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(e => e.PropertyName == "Password");
        result.Errors.ShouldContain(e => e.ErrorMessage == "Password is required.");
    }
}