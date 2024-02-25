namespace Auth.Domain.Users;

public struct UserId
{
    public UserId(int value)
    {
        Value = value;
    }

    public int Value { get; init; }
}