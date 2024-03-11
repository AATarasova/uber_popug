namespace SchemaRegistry.Schemas;

public class EventMeta<TVersion>
{
    public TVersion Version { get; init; }
}