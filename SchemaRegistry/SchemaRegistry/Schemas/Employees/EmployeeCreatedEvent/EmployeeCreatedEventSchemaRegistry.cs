namespace SchemaRegistry.Schemas.Employees.EmployeeCreatedEvent;

public class EmployeeCreatedEventSchemaRegistry : SchemaRegistry
{
    public override string EventName => "EmployeeCreatedEvent";
    public override string FeatureName => "Employees";
    public override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}