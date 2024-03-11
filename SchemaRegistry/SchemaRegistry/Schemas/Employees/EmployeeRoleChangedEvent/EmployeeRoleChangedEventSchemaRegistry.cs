
namespace SchemaRegistry.Schemas.Employees.EmployeeRoleChangedEvent;

public class EmployeeRoleChangedEventSchemaRegistry : SchemaRegistry
{
    protected override string FeatureName => "Employees";
    protected override string EventName => "EmployeeRoleChanged";
    protected override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}