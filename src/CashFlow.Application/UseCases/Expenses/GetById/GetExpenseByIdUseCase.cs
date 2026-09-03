using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using AutoMapper;
using CashFlow.Exception.ExceptionsBase;
namespace CashFlow.Application.UseCases.Expenses.GetById;


public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesRepository;
    private readonly IMapper _mapper;

    public GetExpenseByIdUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper)
    {
        _expensesRepository = expensesRepository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var result = await _expensesRepository.GetById(id);

        if (result is null)
        {
            throw new NotFoundException(ErrorMessages.ExpenseNotFound);
        }

        return _mapper.Map<ResponseExpenseJson>(result);
    }
}