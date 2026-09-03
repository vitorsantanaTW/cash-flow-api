using System.Reflection;
using CashFlow.Application.UseCases.Expenses.Report.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Report.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Report.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "$";
    private const int HEIGHT_OF_TABLE_ROW = 25;
    private const int LEFT_INDENT_OF_TABLE_CELL = 20;
    private readonly IExpensesReadOnlyRepository _expenseRepository;

    public GenerateExpensesReportPdfUseCase(
        IExpensesReadOnlyRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
        GlobalFontSettings.FontResolver = new ExpensesReportFontsResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _expenseRepository.GetByMonth(month);

        if (expenses.Count == 0)
        {

            return Array.Empty<byte>();
        }

        var document = CreateDocument(month);
        var page = CreatePage(document);

        CreatedHeaderWithProfilePhotoAndName(page);

        var totalExpenses = expenses.Sum(expenses => expenses.Amount);

        CreateTotalSpentSection(page: page, month: month, totalExpenses: totalExpenses);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_OF_TABLE_ROW;

            AddExpenseTitle(row.Cells[0], expense.Title);
            AddHeaderForAmount(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_OF_TABLE_ROW;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = LEFT_INDENT_OF_TABLE_CELL;

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddExpenseForAmount(row.Cells[3], expense.Amount);

            if (!string.IsNullOrEmpty(expense.Description))
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_OF_TABLE_ROW;
                descriptionRow.Cells[0].AddParagraph(expense.Description);
                descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.WORK_SANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
                descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = LEFT_INDENT_OF_TABLE_CELL;

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private void AddExpenseForAmount(Cell cell, decimal value)
    {
        cell.AddParagraph($"-{value} {CURRENCY_SYMBOL}");
        cell.Format.Font = new Font { Name = FontHelper.WORK_SANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.LeftIndent = LEFT_INDENT_OF_TABLE_CELL;
    }

    private void AddExpenseTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = LEFT_INDENT_OF_TABLE_CELL;
    }
    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ReportGenerationMessages.Amount);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORK_SANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"{ReportGenerationMessages.ExpensesFor} - {month:Y}";
        document.Info.Author = "CashFlow";

        var style = document.Styles["Normal"];

        style!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }


    private byte[] RenderDocument(Document document)
    {
        var render = new PdfDocumentRenderer
        {
            Document = document
        };

        render.RenderDocument();

        using var stream = new MemoryStream();
        render.PdfDocument.Save(stream, false);

        return stream.ToArray();
    }

    private void CreatedHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();

        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var imagePath = Path.Combine(
             AppContext.BaseDirectory,
                "UseCases",
                "Expenses",
                "Report",
                "Pdf",
                "Logo",
                "logo.jpg"
        );

        row.Cells[0].AddImage(imagePath).Width = 62;
        row.Cells[1].AddParagraph("Hey, Vitor Santana");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(ReportGenerationMessages.TotalSpentIn, month.ToString("Y")) + ": " + totalExpenses.ToString("C");

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
        paragraph.AddLineBreak();


        paragraph.AddFormattedText($"{totalExpenses} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORK_SANS_BLACK, Size = 50 });
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }
}