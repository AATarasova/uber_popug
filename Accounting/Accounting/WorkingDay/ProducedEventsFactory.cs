using System.Text.Json;
using System.Text.Json.Serialization;
using Accounting.Domain.Accounts;
using Accounting.Domain.Transactions;
using SchemaRegistry.Schemas.Accounting.WorkdayCompletedEvent;
using TransactionType = Accounting.Domain.Transactions.TransactionType;
using EventTransactionType = SchemaRegistry.Schemas.Accounting.WorkdayCompletedEvent.EventTransactionType;

namespace Accounting.WorkingDay;

public class ProducedEventsFactory(WorkdayCompletedEventSchemaRegistry workdayCompletedEventSchemaRegistry)
{
    private const string LastSupportedVersion = "V1";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public async Task<string> CreateWorkdayCompletedEvent(IReadOnlyCollection<Account> accounts,
        IReadOnlyCollection<Transaction> transactions, DateTime day)
    {
        var employeeByAccount = accounts.ToDictionary(a => a.Id, a => a.EmployeeId);
        var accountStates = accounts
            .Select(a => new WorkdayCompletedEvent_V1.AccountState
            {
                Balance = a.Balance,
                EmployeeId = a.EmployeeId.Value
            })
            .ToList();
        var sharingTransactions = transactions
            .Where(t => t.TransactionType != TransactionType.SalaryPayment)
            .Select(t => new WorkdayCompletedEvent_V1.Transaction
            {
                TransactionType = ConvertTransactionType(t.TransactionType),
                EmployeeId = employeeByAccount[t.TargetAccountId].Value,
                Sum = t.Sum
            })
            .ToList();

        var producedEvent = new WorkdayCompletedEvent_V1
        {
            Date = day,
            AccountStates = accountStates,
            Transactions = sharingTransactions
        };
        
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        await workdayCompletedEventSchemaRegistry.Validate(serialized, LastSupportedVersion);

        return serialized;
    }
    
    private EventTransactionType ConvertTransactionType(TransactionType type) => type switch
    {
        TransactionType.SalaryPayment => EventTransactionType.SalaryPayment,
        TransactionType.AssignedTaskWithdrawal => EventTransactionType.AssignedTaskWithdrawal,
        TransactionType.CompletedTaskPayment => EventTransactionType.CompletedTaskPayment,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}