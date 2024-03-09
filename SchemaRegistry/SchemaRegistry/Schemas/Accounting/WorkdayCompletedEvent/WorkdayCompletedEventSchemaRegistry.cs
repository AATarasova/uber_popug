namespace SchemaRegistry.Schemas.Accounting.WorkdayCompletedEvent;

public class WorkdayCompletedEventSchemaRegistry : SchemaRegistry
{
    public override string EventName => "WorkdayCompletedEvent";
    public override string FeatureName => "Accounting";
    public override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}