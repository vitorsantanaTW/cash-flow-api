using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Tokens;


namespace CashFlow.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddToken(services, configuration);
        AddRepositories(services);
        AddDbContext(services, configuration);
        services.AddScoped<IPasswordEncrypt, Security.BCrypt>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
      var expirationTimeMinutes = configuration.GetValue<int>("Settings:Jwt:ExpiresInMinutes");
      var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

      services.AddScoped<IAccessTokenGenerator>(provider => new JwtTokenGenerator(signingKey!, (uint)expirationTimeMinutes));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpenseUpdateOnlyRepository, ExpensesRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
       var connectionString = configuration.GetConnectionString("Connection");
       var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));

       services.AddDbContext<CashFlowDbContext>(options => options.UseMySql(connectionString!, serverVersion));
    }
}