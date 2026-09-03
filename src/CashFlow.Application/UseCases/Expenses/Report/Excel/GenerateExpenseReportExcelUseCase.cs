using ClosedXML.Excel;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Extensions;

namespace CashFlow.Application.UseCases.Expenses.Report.Excel;

public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
{

    private const string CURRENCY_SYMBOL = "R$";
    private readonly IExpensesReadOnlyRepository _expenseReportRepository;

    public GenerateExpenseReportExcelUseCase(IExpensesReadOnlyRepository expenseReportRepository)
    {
        _expenseReportRepository = expenseReportRepository;
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _expenseReportRepository.GetByMonth(month);

        if (expenses.Count == 0)
        {
            return [];
        }

        using var workbook = new XLWorkbook();

        workbook.Author = "CashFlow";
        workbook.Style.Font.FontName = "Arial";
        workbook.Style.Font.FontSize = 12;

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        InsertHeader(worksheet);

        var rowNumber = 2;

        foreach (var expense in expenses)
        {
            var row = worksheet.Row(rowNumber);
            rowNumber++;

            row.Cell("A").Value = expense.Title;
            row.Cell("B").Value = expense.Date.ToString("dd/MM/yyyy");
            row.Cell("C").Value = expense.PaymentType.PaymentTypeToString();
            row.Cell("D").Value = expense.Amount;
            row.Cell("D").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL}#,##0.00";
            row.Cell("E").Value = expense.Description;
        }

        worksheet.Columns().AdjustToContents();

        var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ReportGenerationMessages.Title;
        worksheet.Cell("B1").Value = ReportGenerationMessages.Date;
        worksheet.Cell("C1").Value = ReportGenerationMessages.PaymentType;
        worksheet.Cell("D1").Value = ReportGenerationMessages.Amount;
        worksheet.Cell("E1").Value = ReportGenerationMessages.Description;

        var headerRange = worksheet.Range("A1:E1");

        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}