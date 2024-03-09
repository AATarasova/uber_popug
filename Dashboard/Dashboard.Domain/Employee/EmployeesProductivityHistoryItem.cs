namespace Dashboard.Domain.Employee;

public class EmployeesProductivityHistoryItem
{
    public DateTime Date { get; init; }
    public uint EmployeesNumber{ get; init; }
    public uint UnproductiveEmployeesNumber { get; init; }
}