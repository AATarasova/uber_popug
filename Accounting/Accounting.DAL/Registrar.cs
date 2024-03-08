using Accounting.DAL.Repository;
using Accounting.Domain.Accounts;
using Accounting.Domain.Tasks;
using Accounting.Domain.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.DAL;

public static class Registrar
{
    public static void RegisterDAL(this IServiceCollection services)
    {
        services.AddScoped<ITasksRepository, TasksRepository>()
            .AddScoped<IAccountsRepository, AccountsRepository>()
            .AddScoped<ITransactionRepository, TransactionRepository>();
    }
}