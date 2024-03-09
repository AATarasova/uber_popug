using Accounting.Domain.Accounts;
using Accounting.Domain.Tasks;

namespace Accounting.Domain.Payment;

public interface IPaymentService
{
    Task PayForCompletedTask(TaskId taskId, EmployeeId employeeId);
    Task GetPaidForAssignedTask(TaskId taskId, EmployeeId employeeId);
    Task PaySalary();
}