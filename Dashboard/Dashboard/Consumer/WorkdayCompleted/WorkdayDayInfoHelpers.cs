using Dashboard.Domain.Tasks;
using Dashboard.Domain.Transactions;
using Dashboard.Domain.Company;
using Dashboard.Domain.Employee;

namespace Dashboard.Consumer.WorkdayCompleted;

public static class WorkdayDayInfoHelpers
{
    public static EmployeesProductivityHistoryItem GetEmployeesProductivity(WorkdayCompletedEvent workdayCompleted) =>
        new()
        {
            EmployeesNumber = (uint)workdayCompleted.AccountStates.Count,
            UnproductiveEmployeesNumber = (uint)workdayCompleted.AccountStates.Count(a => a.Balance < 0),
            Date = workdayCompleted.Date
        };
    
    public static CompanyAccountHistoryItem GetCompanyAccountInfo(WorkdayCompletedEvent workdayCompleted)
    {
        var assignedTasksCost = (long)workdayCompleted.Transactions
            .Where(t => t.TransactionType == TransactionType.AssignedTaskWithdrawal)
            .Sum(t => (decimal)t.Sum);
        var completedTasksCost = (long)workdayCompleted.Transactions
            .Where(t => t.TransactionType == TransactionType.CompletedTaskPayment)
            .Sum(t => (decimal)t.Sum);
        
        return new CompanyAccountHistoryItem()
        {
            Date = workdayCompleted.Date,
            Balance = assignedTasksCost - completedTasksCost
        };
    }
    
    public static TasksRatingHistoryItem GetTasksRating(WorkdayCompletedEvent workdayCompleted)
    {
        var completedTask = workdayCompleted.Transactions
            .Where(t => t.TransactionType == TransactionType.CompletedTaskPayment)
            .MaxBy(t => t.Sum);
        
        return new TasksRatingHistoryItem
        {
            Date = workdayCompleted.Date,
            MaxCost = completedTask!.Sum
        };
    }
}