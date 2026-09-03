namespace CashFlow.Application.UseCases.Expenses.Register;

using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

public interface IRegisterExpenseUseCase
{
    Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request);
}