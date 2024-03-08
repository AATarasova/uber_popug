namespace Auth.Domain.Users;

public record UserName(string LastName, string FirstName, string? MiddleName)
{
    public string FullName => $"{LastName} {FirstName}{(MiddleName is not null ? $" {MiddleName}" : "")}";
    public string LastNameAndInitials => $"{LastName} {FirstName[0]}.{(MiddleName is not null ? $" {MiddleName[0]}." : "")}";
}