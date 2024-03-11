using Accounting.DAL.Context;
using Accounting.Domain.Accounts;
using Accounting.Domain.Tasks;
using Accounting.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using DbTask = Accounting.Domain.Models.Task;

namespace Accounting.DAL.Repository;

public class TasksRepository(AccountingDbContext dbContext) : ITasksRepository
{
    public async Task<IReadOnlyCollection<Transaction>> ListByDate(DateTime completedDate)
    {
        var transactions =  await Task.FromResult(dbContext.Transactions.Where(t => t.TransactionDate == completedDate));
        return transactions.Select(t => new Transaction
            {
                TargetAccountId = new AccountId(t.TargetAccountId),
                Sum = t.Sum,
                TransactionDate = t.TransactionDate
            })
            .ToList();
    }

    public async Task Add(TaskId taskId)
    {
        var task = new DbTask
        {
            TaskId = taskId.Value,
        };
        await dbContext.Tasks.AddAsync(task);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TaskDto> GetById(TaskId taskId)
    {
        var task = await dbContext.Tasks.FirstAsync(t => t.TaskId == taskId.Value);
        return new TaskDto
        {
            CompletionPrice = task.CompletionPrice,
            AssignmentPrice = task.AssignmentPrice,
            TaskId = taskId
        };
    }
}