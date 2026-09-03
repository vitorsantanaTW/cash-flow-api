namespace Validators.Tests.Expenses.Register;

using CashFlow.Application.UseCases.Expenses;
using CashFlow.Application.UseCases.Expenses.Register;
using CommonTestUtilities.Requests;

using Xunit;
using Shouldly;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Communication.Enums;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }


    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Title_Empty(string? title)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();

        request.Title = title!;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.PropertyName == "Title");
        result.Errors.ShouldContain(e => e.ErrorMessage == ErrorMessages.TitleRequired);
    }

    [Fact]
    public void Error_Date_InTheFuture()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();

        request.Date = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.PropertyName == "Date");
        result.Errors.ShouldContain(e => e.ErrorMessage == ErrorMessages.DateCannotBeInTheFuture);
    }

    [Theory]
    [InlineData((PaymentType)700)]
    [InlineData((PaymentType)(-1))]
    [InlineData((PaymentType)(11))]
    public void Error_Invalid_PaymentType(PaymentType paymentType)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();

        request.PaymentType = paymentType;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.PropertyName == "PaymentType");
        result.Errors.ShouldContain(e => e.ErrorMessage == ErrorMessages.InvalidPaymentType);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Error_Amount_LessThanOrEqualToZero(decimal amount)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();

        request.Amount = amount;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.PropertyName == "Amount");
        result.Errors.ShouldContain(e => e.ErrorMessage == ErrorMessages.AmountGreaterThanZero);
    }
}