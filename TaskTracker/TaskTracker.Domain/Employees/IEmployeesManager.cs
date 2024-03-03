using TaskTracker.Domain.Employees.Dto;

namespace TaskTracker.Domain.Employees;

public interface IEmployeesManager
{
    Task Create(Employee employee);
    Task Update(EmployeeId employee, Role role);
    Task Delete(EmployeeId employee);
    Task<IReadOnlyCollection<EmployeeId>> ListAllDevelopers();
    Task<IReadOnlyCollection<EmployeeId>> ListAll();
}