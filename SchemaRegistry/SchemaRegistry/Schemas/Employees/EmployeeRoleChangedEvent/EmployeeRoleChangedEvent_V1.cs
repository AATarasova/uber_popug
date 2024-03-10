namespace SchemaRegistry.Schemas.Employees.EmployeeRoleChangedEvent;

public class EmployeeRoleChangedEvent_V1
{
    public Guid EmployeeId { get; set; }
    public Role Role { get; init; }
}