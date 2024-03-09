namespace Dashboard.Domain.Models;

public class CompanyAccountHistoryItem
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public long Balance { get; init; }
}