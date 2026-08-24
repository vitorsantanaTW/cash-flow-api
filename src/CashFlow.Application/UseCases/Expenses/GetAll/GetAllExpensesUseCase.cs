using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{

    private readonly IExpensesReadOnlyRepository _expensesRepository;
    private readonly IMapper _mapper;

    public GetAllExpensesUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper)
    {
        _expensesRepository = expensesRepository;
        _mapper = mapper;
    }
    public async Task<ResponseExpensesJson> Execute()
    {
        var result = await _expensesRepository.GetAll();
       return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}