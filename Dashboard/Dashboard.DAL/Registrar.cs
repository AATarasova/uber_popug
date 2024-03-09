using Dashboard.DAL.Repositories;
using Dashboard.Domain.Company;
using Dashboard.Domain.Employee;
using Microsoft.Extensions.DependencyInjection;

namespace Dashboard.DAL;

public static class Registrar
{
    public static void RegisterDAL(this IServiceCollection services)
    {
        services.AddScoped<ICompanyAccountRepository, CompanyAccountRepository>()
            .AddScoped<IEmployeeProductivityRepository, EmployeeProductivityRepository>()
            .AddScoped<ITasksRatingRepository, TasksRatingRepository>();
    }
}