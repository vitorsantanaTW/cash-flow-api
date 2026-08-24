using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseUseCase(IExpensesWriteOnlyRepository expenseRepository, IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var expense = await _expenseRepository.Delete(id);

        if (expense == false)
        {
            throw new NotFoundException(ErrorMessages.ExpenseNotFound);
        }

        await _unitOfWork.Commit();
    }
}