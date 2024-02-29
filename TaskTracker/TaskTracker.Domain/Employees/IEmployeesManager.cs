using TaskTracker.Domain.Employees.Dto;

namespace TaskTracker.Domain.Employees;

public interface IEmployeesManager
{
    Task Create(Employee employee);
    Task<IReadOnlyCollection<EmployeeId>> ListAllDevelopers();
}