
using CashFlow.Domain.Entities;
using Moq;
using CashFlow.Domain.Repositories.User;
namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistsActiveUserWithEmail(string email)
    {
        _repository.Setup(userReadOnly => userReadOnly.ExistsActiveUserWithEmail(email)).ReturnsAsync(true);
    }
    
    public UserReadOnlyRepositoryBuilder GetActiveUserByEmail(User user)
    {
        _repository.Setup(userReadOnly => userReadOnly.GetActiveUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}