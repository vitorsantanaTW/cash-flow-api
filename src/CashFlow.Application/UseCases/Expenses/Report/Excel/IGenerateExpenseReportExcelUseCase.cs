namespace CashFlow.Application.UseCases.Expenses.Report.Excel;

public interface IGenerateExpenseReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}