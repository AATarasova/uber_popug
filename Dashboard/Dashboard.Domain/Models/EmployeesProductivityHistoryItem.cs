namespace Dashboard.Domain.Models;

public class EmployeesProductivityHistoryItem
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public uint EmployeesNumber{ get; init; }
    public uint UnproductiveEmployeesNumber { get; init; }
}