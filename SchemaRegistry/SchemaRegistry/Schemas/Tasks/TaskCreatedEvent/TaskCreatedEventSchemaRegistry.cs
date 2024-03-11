namespace SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;

public class TaskCreatedEventSchemaRegistry : SchemaRegistry
{
    protected override string EventName => "TaskCreatedEvent";
    protected override string FeatureName => "Tasks";
    protected override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}