using CashFlow.Application.UseCases.User.Login;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.User.Login;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IPasswordEncrypt _passwordEncrypt;
    private readonly IAccessTokenGenerator _tokenGenerator;

    public DoLoginUseCase(
        IUserReadOnlyRepository userRepository,
        IPasswordEncrypt passwordEncrypt,
        IAccessTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordEncrypt = passwordEncrypt;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userRepository.GetActiveUserByEmail(request.Email);

        if (user is null)
        {
            throw new InvalidLoginException();
        }

        var validPassword = _passwordEncrypt.Verify(request.Password, user.Password);

        if (!validPassword)
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }
}