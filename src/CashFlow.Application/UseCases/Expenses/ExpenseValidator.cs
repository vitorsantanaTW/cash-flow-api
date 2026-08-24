using FluentValidation;
using CashFlow.Communication.Requests;
using CashFlow.Exception.ExceptionsBase;
namespace CashFlow.Application.UseCases.Expenses.Register;

public class ExpenseValidator: AbstractValidator<RequestExpenseJson>
{
    public ExpenseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(ErrorMessages.TitleRequired);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(ErrorMessages.AmountGreaterThanZero);

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ErrorMessages.DateCannotBeInTheFuture);

        RuleFor(x => x.PaymentType)
            .IsInEnum().WithMessage(ErrorMessages.InvalidPaymentType);
    }
}