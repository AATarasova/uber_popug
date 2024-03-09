using Dashboard.Consumer.WorkdayCompleted;
using Dashboard.Domain.Company;
using Dashboard.Domain.Employee;

namespace Dashboard.Consumer;

public class ConsumerBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var thread = new Thread( async () => await InitAccountsStateConsumer(stoppingToken));
        thread.Start();
        thread.Join();
        return Task.CompletedTask;
    }
    
    private async Task InitAccountsStateConsumer(CancellationToken cancellationToken)
    {
        Thread.CurrentThread.IsBackground = true;
        logger.LogInformation("accounts-state consumer running at: {time}", DateTimeOffset.Now);
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            await eventConsumer.SubscribeTopic<WorkdayCompletedEvent>("employees-streaming", HandleWorkdayCompleted, cancellationToken);
        }

        logger.LogInformation("accounts-state consumer stopped at: {time}", DateTimeOffset.Now);
    }

    private async Task HandleWorkdayCompleted(WorkdayCompletedEvent workdayCompleted)
    {
        using var scope = serviceScopeFactory.CreateScope();
        
        // employeeProductivity
        var employeeProductivityRepository = scope.ServiceProvider.GetRequiredService<IEmployeeProductivityRepository>();
        var employeeDayStatistic = WorkdayDayInfoHelpers.GetEmployeesProductivity(workdayCompleted);
        await employeeProductivityRepository.Add(employeeDayStatistic);
        
        // companyAccount
        var companyAccountRepository = scope.ServiceProvider.GetRequiredService<ICompanyAccountRepository>();
        var companyAccountInfo = WorkdayDayInfoHelpers.GetCompanyAccountInfo(workdayCompleted);
        await companyAccountRepository.Add(companyAccountInfo);
        
        // tasksRating
        var tasksRatingRepository = scope.ServiceProvider.GetRequiredService<ITasksRatingRepository>();
        var tasksInfo = WorkdayDayInfoHelpers.GetTasksRating(workdayCompleted);
        await tasksRatingRepository.Add(tasksInfo);
    }
}