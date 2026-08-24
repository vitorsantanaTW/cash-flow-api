namespace CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Responses;
public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpenseJson> Execute(long id);
}