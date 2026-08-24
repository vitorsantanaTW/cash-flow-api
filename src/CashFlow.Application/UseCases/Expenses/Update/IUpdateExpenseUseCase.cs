namespace CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Communication.Requests;
public interface IUpdateExpenseUseCase
{
    Task Execute(long id, RequestExpenseJson request);
}