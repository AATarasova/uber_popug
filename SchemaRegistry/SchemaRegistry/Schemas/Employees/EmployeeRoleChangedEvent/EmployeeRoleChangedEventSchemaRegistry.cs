
namespace SchemaRegistry.Schemas.Employees.EmployeeRoleChangedEvent;

public class EmployeeRoleChangedEventSchemaRegistry : SchemaRegistry
{
    public override string FeatureName => "Employees";
    public override string EventName => "EmployeeRoleChanged";
    public override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}