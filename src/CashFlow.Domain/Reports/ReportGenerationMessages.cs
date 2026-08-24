using System.Reflection;
using System.Resources;
using System.Globalization;
namespace CashFlow.Domain.Reports;

public static class ReportGenerationMessages
{
    private static readonly ResourceManager ResourceManager =
        new(
            "CashFlow.Domain.Reports.ResourceReportGenerationMessages",
            Assembly.GetExecutingAssembly());

    public static string Title =>
        ResourceManager.GetString("TITLE", CultureInfo.CurrentUICulture)!;

    public static string Description =>
        ResourceManager.GetString("DESCRIPTION", CultureInfo.CurrentUICulture)!;

    public static string Amount =>
        ResourceManager.GetString("AMOUNT", CultureInfo.CurrentUICulture)!;

    public static string Date =>
        ResourceManager.GetString("DATE", CultureInfo.CurrentUICulture)!;

    public static string PaymentType =>
        ResourceManager.GetString("PAYMENT_TYPE", CultureInfo.CurrentUICulture)!;

    public static string ExpensesFor =>
        ResourceManager.GetString("EXPENSES_FOR", CultureInfo.CurrentUICulture)!;

    public static string TotalSpentIn =>
        ResourceManager.GetString("TOTAL_SPENT_IN", CultureInfo.CurrentUICulture)!;
}