using System.Reflection;
using System.Resources;
using System.Globalization;
namespace CashFlow.Exception.ExceptionsBase;

public static class ErrorMessages
{
    private static readonly ResourceManager ResourceManager =
        new(
            "CashFlow.Exception.ResourceErrorMessages",
            Assembly.GetExecutingAssembly());

    public static string UnknownError =>
        ResourceManager.GetString("UNKNOWN_ERROR", CultureInfo.CurrentUICulture)!;

    public static string TitleRequired =>
        ResourceManager.GetString("TITLE_REQUIRED", CultureInfo.CurrentUICulture)!;

    public static string AmountGreaterThanZero =>
        ResourceManager.GetString("AMOUNT_GREATER_THAN_ZERO", CultureInfo.CurrentUICulture)!;

    public static string DateCannotBeInTheFuture =>
        ResourceManager.GetString("DATE_CANNOT_BE_IN_THE_FUTURE", CultureInfo.CurrentUICulture)!;

    public static string InvalidPaymentType =>
        ResourceManager.GetString("INVALID_PAYMENT_TYPE", CultureInfo.CurrentUICulture)!;

    public static string ExpenseNotFound =>
        ResourceManager.GetString("EXPENSE_NOT_FOUND", CultureInfo.CurrentUICulture)!;
}