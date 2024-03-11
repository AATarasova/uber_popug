using Accounting.Domain.Accounts;
using Accounting.Domain.Tasks;
using Accounting.Domain.Transactions;

namespace Accounting.Domain.Payment;

public class PaymentService(ITransactionRepository transactionRepository,
    IAccountsRepository accountsRepository,
    ITasksRepository tasksRepository) : IPaymentService
{
    public async Task PayForCompletedTask(TaskId taskId, EmployeeId employeeId)
    {
        var task = await tasksRepository.GetById(taskId);

        var transactionInfo = new AddTransactionDto
        {
            TargetEmployeeId = employeeId,
            Sum = task.CompletionPrice,
            SourceTaskId = taskId,
            TransactionType = TransactionType.CompletedTaskPayment,
        };
        await transactionRepository.Add(transactionInfo);
    }

    public async Task GetPaidForAssignedTask(TaskId taskId, EmployeeId employeeId)
    { 
        var task = await tasksRepository.GetById(taskId);

        var transactionInfo = new AddTransactionDto
        {
            TargetEmployeeId = employeeId,
            Sum = task.AssignmentPrice,
            SourceTaskId = taskId,
            TransactionType = TransactionType.AssignedTaskWithdrawal,
        };
        await transactionRepository.Add(transactionInfo);
    }

    public async Task PaySalary()
    {
        var accounts = await accountsRepository.ListAll();
        var transactions = accounts
            .Where(a => a.Balance > 0)
            .Select(a => new AddTransactionDto
            {
                TargetEmployeeId = a.EmployeeId,
                Sum = (ulong)a.Balance,
                TransactionType = TransactionType.SalaryPayment,
            })
            .ToList();
        
        await transactionRepository.Add(transactions);
    }
}