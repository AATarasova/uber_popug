using Auth.Domain.Roles;

namespace Auth.Domain.Users;

public class User
{
    public UserId Id { get; set; }
    public Guid PublicId { get; set; }
    public UserName UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role Role { get; set; }
}