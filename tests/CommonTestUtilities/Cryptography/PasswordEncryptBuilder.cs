using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncryptBuilder
{
    private readonly Mock<IPasswordEncrypt> _mock;
    
    public PasswordEncryptBuilder()
    {
        _mock = new Mock<IPasswordEncrypt>();
        _mock.Setup(f => f.Encrypt(It.IsAny<string>())).Returns("mocked_encrypted_password");
    }

    public PasswordEncryptBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
        {
            _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);
        }

        return this;
    }

    public IPasswordEncrypt Build() => _mock.Object;
}