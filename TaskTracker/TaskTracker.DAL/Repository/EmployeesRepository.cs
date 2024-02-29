using TaskTracker.DAL.Context;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Employees.Dto;
using DbEmployee = TaskTracker.Domain.Models.Employee;

namespace TaskTracker.DAL.Repository;

internal class EmployeesRepository(TaskTrackerDbContext dbContext) : IEmployeesManager
{
    public async Task Create(Employee employee)
    {
        var dbEmployee = new DbEmployee()
        {
            Id = employee.Id.Value,
            Role = employee.Role
        };
        
        await dbContext.AddAsync(dbEmployee);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<EmployeeId>> ListAllDevelopers()
    {
        var developers = dbContext.Employees
            .Where(e => e.Role == Role.Developer)
            .AsEnumerable()
            .Select(Convert)
            .ToList();
        return await Task.FromResult(developers);
    }

    private EmployeeId Convert(DbEmployee employee) => new(employee.Id);

}