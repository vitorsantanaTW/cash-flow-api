using CashFlow.Application.UseCases.User.Register;
using CommonTestUtilities.Requests;
using Shouldly;
using Xunit;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Token;
using CommonTestUtilities.Cryptography;

namespace UseCases.Tests.User;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var useCase = CreateUseCase();

        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Name.ShouldBe(request.Name);
        response.Token.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain("Name is required.");
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase(request.Email);
        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain("Email already exists");
    }


    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var passwordEncrypt = new PasswordEncryptBuilder().Build();
        var readRepository = new UserReadOnlyRepositoryBuilder();

        if (!string.IsNullOrWhiteSpace(email))
        {
            readRepository.ExistsActiveUserWithEmail(email);
        }

        return new RegisterUserUseCase(mapper, passwordEncrypt, readRepository.Build(), writeRepository, unitOfWork, accessTokenGenerator);
    }
}
