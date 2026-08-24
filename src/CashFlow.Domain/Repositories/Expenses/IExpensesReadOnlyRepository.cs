namespace CashFlow.Domain.Repositories.Expenses;

using CashFlow.Domain.Entities;

public interface IExpensesReadOnlyRepository
{
    Task<Expense?> GetById(long id);
    Task<List<Expense>> GetAll();

    Task<List<Expense>> GetByMonth(DateOnly month);
}