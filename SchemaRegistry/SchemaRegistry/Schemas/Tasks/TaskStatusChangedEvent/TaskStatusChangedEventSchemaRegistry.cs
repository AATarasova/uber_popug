namespace SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent;

public class TaskStatusChangedEventSchemaRegistry : SchemaRegistry
{
    public override string EventName => "TaskStatusChangedEvent";
    public override string FeatureName => "Tasks";
    public override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}