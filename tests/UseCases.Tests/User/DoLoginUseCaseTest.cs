using CashFlow.Application.UseCases.User.Login;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using Shouldly;
using Xunit;

namespace UseCases.Tests.User;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestLoginJsonBuilder.Build();
     
        var user = UserBuilder.Build();
        
        request.Email =  user.Email;
        var useCase = CreateUseCase(user, request.Password);
        
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Token.ShouldNotBeNull().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var request = RequestLoginJsonBuilder.Build();
     
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);
        
        var act = async () => await useCase.Execute(request);
        
        var result = await act().ShouldThrowAsync<InvalidLoginException>();

        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain("Email or password is invalid.");
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        var request = RequestLoginJsonBuilder.Build();
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        request.Email =  user.Email;
        
        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<InvalidLoginException>();
        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain("Email or password is invalid.");
    }

    private DoLoginUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? password = null)
    {
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();
        var passwordEncrypt = new PasswordEncryptBuilder().Verify(password: password).Build();
        var readRepository = new UserReadOnlyRepositoryBuilder().GetActiveUserByEmail(user).Build();
        
        return new DoLoginUseCase(readRepository, passwordEncrypt, tokenGenerator);
    }
}