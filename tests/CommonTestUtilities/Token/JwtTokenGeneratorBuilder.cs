using Moq;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Entities;

namespace CommonTestUtilities.Token;


public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(accessTokenGenerator => accessTokenGenerator.Generate(It.IsAny<User>()))
        .Returns("mocked_token");

        return mock.Object;
    }
}