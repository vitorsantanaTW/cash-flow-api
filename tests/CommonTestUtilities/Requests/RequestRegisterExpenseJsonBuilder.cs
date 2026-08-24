namespace CommonTestUtilities.Requests;

using CashFlow.Communication.Requests;
using CashFlow.Communication.Enums;
using Bogus;

public class RequestRegisterExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        return new Faker<RequestExpenseJson>()
            .RuleFor(x => x.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(x => x.Description, faker => faker.Lorem.Sentence())
            .RuleFor(x => x.Date, faker => faker.Date.Past())
            .RuleFor(x => x.Title, faker => faker.Commerce.ProductName())
            .RuleFor(x => x.PaymentType, faker => faker.PickRandom<PaymentType>())
            .Generate();
    }
}