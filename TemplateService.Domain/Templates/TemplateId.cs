namespace TemplateService.Domain.Templates;

public struct TemplateId
{
    public TemplateId(int value)
    {
        this.Value = value;
    }

    public int Value { get; init; }
}