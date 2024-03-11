namespace SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent;

public class TaskStatusChangedEventSchemaRegistry : SchemaRegistry
{
    protected override string EventName => "TaskStatusChangedEvent";
    protected override string FeatureName => "Tasks";
    protected override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}