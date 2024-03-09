using System.Reflection;
using NJsonSchema;

namespace SchemaRegistry.Schemas;

public abstract class SchemaRegistry
{
    public string SchemaPath =>
        Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            "Schemas",
            FeatureName,
            EventName);

    public abstract string EventName { get; }
    public abstract string FeatureName { get; }
    public abstract IReadOnlyCollection<string> SupportedVersions { get; }

    public async Task<string> GetSchemaByVersion(string version)
    {
        return SupportedVersions.Contains(version)
            ? (await JsonSchema.FromFileAsync(Path.Combine(SchemaPath, $"{EventName}_{version}.json"))).ToJson()
            : throw new InvalidOperationException("Invalid schema version");
    }
}