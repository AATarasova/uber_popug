namespace SchemaRegistry.Schemas.Accounting.WorkdayCompletedEvent;

public class WorkdayCompletedEventSchemaRegistry : SchemaRegistry
{
    protected override string EventName => "WorkdayCompletedEvent";
    protected override string FeatureName => "Accounting";
    protected override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}