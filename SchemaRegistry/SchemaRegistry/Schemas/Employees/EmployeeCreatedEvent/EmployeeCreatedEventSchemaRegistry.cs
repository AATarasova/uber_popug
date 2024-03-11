namespace SchemaRegistry.Schemas.Employees.EmployeeCreatedEvent;

public class EmployeeCreatedEventSchemaRegistry : SchemaRegistry
{
    protected override string EventName => "EmployeeCreatedEvent";
    protected override string FeatureName => "Employees";
    protected override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}