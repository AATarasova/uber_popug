using System.Reflection;
using NJsonSchema;

namespace SchemaRegistry.Schemas;

public abstract class SchemaRegistry
{
    private string SchemaPath =>
        Path.Combine(
            "C:\\Developer\\uber_popug\\SchemaRegistry\\SchemaRegistry",
            "Schemas",
            FeatureName,
            EventName);

    protected abstract string EventName { get; }
    protected abstract string FeatureName { get; }
    protected abstract IReadOnlyCollection<string> SupportedVersions { get; }

    public async Task<string> GetSchemaByVersion(string version)
    {
        return SupportedVersions.Contains(version)
            ? (await JsonSchema.FromFileAsync(Path.Combine(SchemaPath, $"{EventName}_{version}.json"))).ToJson()
            : throw new InvalidOperationException("Invalid schema version");
    }

    public async Task Validate(string value, string version)
    {
        var schema = await GetSchemaByVersion(version);
        var validationErrors = JsonSchema.FromJsonAsync(schema).Result.Validate(value);

        if (validationErrors.Any())
        {
            var errors = string.Join(", ", validationErrors.Select(e => $"{e.Property}: {e.Kind}"));
            throw new InvalidCastException($"Event {EventName} not match to version {version}: {errors}");
        }
    }
}