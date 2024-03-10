namespace SchemaRegistry.Schemas.Employees.EmployeeCreatedEvent;

public class EmployeeCreatedEvent_V1
{
    public Guid EmployeeId { get; set; }
    public Role Role { get; init; }
}