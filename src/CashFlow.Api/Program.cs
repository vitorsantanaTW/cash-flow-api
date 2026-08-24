using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Infrastructure;
using CashFlow.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationDependencies();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

await MigrateDataBase();

app.Run();

async Task MigrateDataBase()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DatabaseMigration.MigrateDatabase(scope.ServiceProvider);
}

