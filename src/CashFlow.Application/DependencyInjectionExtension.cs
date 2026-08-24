using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.Delete;
using Microsoft.Extensions.DependencyInjection;
using CashFlow.Application.AutoMapper;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Application.UseCases.Expenses.Report.Excel;
using CashFlow.Application.UseCases.Expenses.Report.Pdf;
using CashFlow.Application.UseCases.User.Register;
public static class DependencyInjectionExtension
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddAutoMapperDependencies();
        services.AddUseCasesDependencies();
    }

    private static void AddAutoMapperDependencies(this IServiceCollection services)
    {
        services.AddAutoMapper(config => config.AddProfile(typeof(AutoMapping)));
    }

    private static void AddUseCasesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
        services.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
        services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
        services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
        services.AddScoped<IGenerateExpenseReportExcelUseCase, GenerateExpenseReportExcelUseCase>();
        services.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    
    }
}