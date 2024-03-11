using Dashboard.Domain.Tasks;
using Dashboard.Domain.Company;
using Dashboard.Domain.Employee;
using SchemaRegistry.Schemas.Accounting.WorkdayCompletedEvent;

namespace Dashboard.Consumer.WorkdayCompleted;

public static class WorkdayDayInfoHelpers
{
    public static EmployeesProductivityHistoryItem GetEmployeesProductivity(WorkdayCompletedEvent_V1 workdayCompleted) =>
        new()
        {
            EmployeesNumber = (uint)workdayCompleted.AccountStates.Count,
            UnproductiveEmployeesNumber = (uint)workdayCompleted.AccountStates.Count(a => a.Balance < 0),
            Date = workdayCompleted.Date
        };
    
    public static CompanyAccountHistoryItem GetCompanyAccountInfo(WorkdayCompletedEvent_V1 workdayCompleted)
    {
        var assignedTasksCost = (long)workdayCompleted.Transactions
            .Where(t => t.TransactionType == EventTransactionType.AssignedTaskWithdrawal)
            .Sum(t => (decimal)t.Sum);
        var completedTasksCost = (long)workdayCompleted.Transactions
            .Where(t => t.TransactionType == EventTransactionType.CompletedTaskPayment)
            .Sum(t => (decimal)t.Sum);
        
        return new CompanyAccountHistoryItem()
        {
            Date = workdayCompleted.Date,
            Balance = assignedTasksCost - completedTasksCost
        };
    }
    
    public static TasksRatingHistoryItem GetTasksRating(WorkdayCompletedEvent_V1 workdayCompleted)
    {
        var completedTask = workdayCompleted.Transactions
            .Where(t => t.TransactionType == EventTransactionType.CompletedTaskPayment)
            .MaxBy(t => t.Sum);
        
        return new TasksRatingHistoryItem
        {
            Date = workdayCompleted.Date,
            MaxCost = completedTask!.Sum
        };
    }
}