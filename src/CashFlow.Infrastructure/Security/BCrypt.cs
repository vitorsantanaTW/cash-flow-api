using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security;

public class BCrypt : IPasswordEncrypt
{
    public string Encrypt(string password)
    {
        string passwordHashed = BC.HashPassword(password);
        return passwordHashed;
    }

    public bool Verify(string password, string passwordHashed)
    {
        return BC.Verify(password, passwordHashed);
    }
}