using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => "Money",
            PaymentType.CreditCard => "Credit Card",
            PaymentType.DebitCard => "Debit Card",
            _ => string.Empty
        };
    }

}