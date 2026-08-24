using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesWriteOnlyRepository
{
    Task AddAsync(Expense expense);
    /// <summary>
    /// Deletes an expense by its ID.
    /// Returns true if the expense was found and deleted, false if not found.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Delete(long id);
}