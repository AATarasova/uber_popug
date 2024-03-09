namespace Accounting.Domain.Accounts;

public struct AccountId(int value)
{
    public int Value { get; init; } = value;
}
