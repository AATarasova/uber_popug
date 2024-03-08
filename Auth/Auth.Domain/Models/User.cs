using Auth.Domain.Roles;

namespace Auth.Domain.Models;

internal class User
{
    public int UserId { get; set; }
    public Guid PublicId { get; set; }
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role Role { get; set; }
}