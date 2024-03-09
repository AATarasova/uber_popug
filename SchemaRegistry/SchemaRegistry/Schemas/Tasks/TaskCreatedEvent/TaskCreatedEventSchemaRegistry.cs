namespace SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;

public class TaskCreatedEventSchemaRegistry : SchemaRegistry
{
    public override string EventName => "TaskCreatedEvent";
    public override string FeatureName => "Tasks";
    public override IReadOnlyCollection<string> SupportedVersions => new[] { "V1" };
}