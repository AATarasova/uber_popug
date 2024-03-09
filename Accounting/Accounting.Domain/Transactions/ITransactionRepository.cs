namespace Accounting.Domain.Transactions;

public interface ITransactionRepository
{
    Task Add(AddTransactionDto transactionInfo);
    Task Add(IReadOnlyCollection<AddTransactionDto> transactionInfo);
    Task<IReadOnlyCollection<Transaction>> ListByDate(DateTime completedDate);
}