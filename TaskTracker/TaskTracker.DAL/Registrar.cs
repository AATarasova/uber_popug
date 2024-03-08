using Microsoft.Extensions.DependencyInjection;
using TaskTracker.DAL.Repository;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Tasks;

namespace TaskTracker.DAL;

public static class Registrar
{
    public static void RegisterDAL(this IServiceCollection services)
    {
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<IEmployeesManager, EmployeesRepository>();
    }
}