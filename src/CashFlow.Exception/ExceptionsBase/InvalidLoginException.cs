using System;
using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

public class InvalidLoginException : CashFlowException
{
    public InvalidLoginException() : base("Email or password is invalid.")
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}